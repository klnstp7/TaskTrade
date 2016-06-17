using KafkaBusClient;
using Peacock.PEP.Service.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KafkaBusClient.Consumer;
using System.Threading;
using Peacock.PEP.Model.ApiModel;
using RabbitMQ.Client;
using log4net;
using RabbitMQ.Client.Events;

namespace Peacock.PEP.Service
{
    public class KafkaMQService : SingModel<KafkaMQService>
    {
        private KafkaMQService()
        { }
        #region Kafka 消息队列服务器配置
        //读取队列服务器地址
        //private readonly string ReadHostName = GetMqConfig("ReadHostName");
        ////端口号
        //private readonly int ReadPort = int.Parse(GetMqConfig("ReadPort"));
        ////分区
        //private readonly int ReadPartitionId = int.Parse(GetMqConfig("ReadPartitionId"));
        ////消息读取主题
        //private readonly string ReadTopicName = GetMqConfig("ReadTopicName");


        ////写入队列服务器地址
        //private readonly string WirteHostName = GetMqConfig("WirteHostName");
        ////端口号
        //private readonly int WritePort = int.Parse(GetMqConfig("WritePort"));
        ////分区
        //private readonly int WritePartitionId = int.Parse(GetMqConfig("WritePartitionId"));
        ////消息写入主题
        //private readonly string WriteTopicName = GetMqConfig("WriteTopicName");

        #endregion

        #region RabbitMq 队列配置

        #endregion

        /// <summary>
        /// 发送消息（RabbitMq）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        private void SendMessageQueue<T>(T Data) where T : class
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = GetMqConfig("WriteHostName");
                factory.Port = int.Parse(GetMqConfig("WritePort"));
                factory.UserName = GetMqConfig("WriteUser");
                factory.Password = GetMqConfig("WritePassWord");
                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        channel.QueueDeclare(GetMqConfig("WriteTopicName"), true, false, false, null);

                        string jsonStr = JsonConvert.SerializeObject(Data);
                        byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);

                        //设置消息持久化
                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        channel.BasicPublish("", GetMqConfig("WriteTopicName"), properties, bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("LogExceptionRabbitMQSend").Error("消息发送失败,消息内容:" + JsonConvert.SerializeObject(Data) + ",错误消息:" + ex.ToString());
            }
        }

        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public short SendMessage<T>(T Data) where T : class
        {
            Task.Factory.StartNew(() => SendMessageQueue<T>(Data));
            //SendMessageQueue<T>(Data);
            //var busConnector = new KafkaBusConnector(WirteHostName, WritePort, "PEPSimple Client CallBackData");
            //推送消息调用
            //var ErrorCode = busConnector.Produce(WriteTopicName, WritePartitionId, JsonConvert.SerializeObject(Data));
            return 0;
        }

        /// <summary>
        /// 启动线程接受消息
        /// </summary>
        public void StartMqReceiveData()
        {
            Task.Factory.StartNew(() => ReceiveDataRabbitMq());
        }

        public void ReceiveDataRabbitMq()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = GetMqConfig("ReadHostName");
            factory.Port = int.Parse(GetMqConfig("ReadPort"));
            factory.UserName = GetMqConfig("ReadUser");
            factory.Password = GetMqConfig("ReadPassWord");
            using (IConnection conn = factory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                    channel.QueueDeclare(GetMqConfig("ReadTopicName"), true, false, false, null);

                    //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                    channel.BasicQos(0, 1, false);
                    //channel.BasicGet();

                    //在队列上定义一个消费者
                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                    //消费队列，并设置应答模式为程序主动应答
                    channel.BasicConsume(GetMqConfig("ReadTopicName"), false, consumer);
                    while (true)
                    {
                        //阻塞函数，获取队列中的消息
                        BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        byte[] bytes = ea.Body;
                        string str = Encoding.UTF8.GetString(bytes);
                        try
                        {
                            LogManager.GetLogger("LogExceptionRabbitMQReceive").Error("接收到消息,消息内容:" + str);
                            ReceiveMessage(str);
                            //channel.BasicAck(ea.DeliveryTag, true);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger("LogExceptionRabbitMQReceive").Error("消息接受失败,消息内容:" + str + ",错误消息:" + ex.ToString());
                        }
                        finally
                        {
                            channel.BasicAck(ea.DeliveryTag, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void ReceiveData()
        {
            //var busConnector = new KafkaBusConnector(ReadHostName, ReadPort, "PEPSimple Client ReadData");
            //IKafkaMessageConsumer consumer = new MessageConsumer();

            //偏移量设置为＜0会记录上次读取到的位置,并且本次只会从上次读取的位置开始读取
            //consumer.Start(busConnector, ReadTopicName, ReadPartitionId, 5, ReceiveMessage);
            //长连接，
            //while (true)
            //{
            //Thread.Sleep(100);//5秒获取一次
            //}
            //consumer.Stop();
        }

        /// <summary>
        /// 接收并处理，遍历消息
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveMessage(string message)
        {
            ReceiveDataEntity Data = JsonConvert.DeserializeObject<ReceiveDataEntity>(message.ToString());
            var EnumList = Enum.GetValues(typeof(BussinessType));
            bool isExists = false;
            foreach (var i in EnumList)
            {
                if (Data.bussinessType == i.ToString())
                {
                    isExists = true;
                    break;
                }
            }
            if (!isExists)
            {
                SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = Data.bussinessId,
                    businessType = "",
                    bussinessForm = new BussinessForm
                    {
                        message = Data.bussinessType + "不存在业务类型枚举范围内，请检查bussinessType",
                        success = false
                    }
                });
                return;
            }
            //根据业务类型调用不同方法
            var bussinessType = (BussinessType)Enum.Parse(typeof(BussinessType), Data.bussinessType);
            switch (bussinessType)
            {
                case BussinessType.预评估:
                case BussinessType.信息补全:
                case BussinessType.正式委托:
                case BussinessType.预评估转正式:
                    var OnlineData = JsonConvert.DeserializeObject<OnlineBusinessApiModel>(Data.bussinessForm.ToString());
                    OnlineBusinessService.Instance.SaveOnLineDataByKafkaMq(OnlineData, Data.dataSource, Data.bussinessId, bussinessType);
                    break;
                case BussinessType.人工询价:
                    var PersonInquriy = JsonConvert.DeserializeObject<OnlineInquiryApiModel>(Data.bussinessForm.ToString());
                    OnlineBusinessService.Instance.SaveOnlineInquiryByKafkaMq(PersonInquriy, Data.bussinessId, Data.dataSource, bussinessType);
                    break;
                case BussinessType.附件上传:
                case BussinessType.附件更新:
                    var docData = JsonConvert.DeserializeObject<List<OnlineBusinessDocmentModel>>(Data.bussinessForm.ToString());
                    ProjectDocumentService.Instance.UploadOnlineDocment(docData, Data.bussinessId, bussinessType);
                    break;
                case BussinessType.业务终止:
                    var StopData = JsonConvert.DeserializeObject<OnlineBusinessStopApiModel>(Data.bussinessForm.ToString());
                    OnlineBusinessService.Instance.StopOnlineBusinessByKafkaMq(StopData, Data.bussinessId);
                    break;
                case BussinessType.预约看房:
                case BussinessType.实勘完成:
                    OnlineBusinessService.Instance.WriteFeedBackKafkaMQ(Data.bussinessId, bussinessType, Data.bussinessForm.ToString());
                    break;
            }
        }

        /// <summary>
        /// 发送消息给外采
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageWaicaiQueue(WaicaiMessage msg)
        {
            Task.Factory.StartNew(() => SendMessageWaicaiQueuePrivate(msg));
        }
        private void SendMessageWaicaiQueuePrivate(WaicaiMessage msg)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = GetMqWaicaiConfig("HostName");
                factory.Port = int.Parse(GetMqWaicaiConfig("Port"));
                factory.UserName = GetMqWaicaiConfig("UserName");
                factory.Password = GetMqWaicaiConfig("Password");
                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        channel.QueueDeclare(GetMqWaicaiConfig("QueueName"), true, false, false, null);

                        string jsonStr = JsonConvert.SerializeObject(msg);
                        byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);

                        //设置消息持久化
                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        channel.BasicPublish("", GetMqWaicaiConfig("QueueName"), properties, bytes);
                        LogManager.GetLogger("LogExceptionAttribute").Error("消息发送外采消息成功,消息内容:" + JsonConvert.SerializeObject(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("LogExceptionAttribute").Error("消息发送失败,消息内容:" + JsonConvert.SerializeObject(msg) + ",错误消息:" + ex.ToString());
            }
        }

        /// <summary>
        /// 读取外采消息配置
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string GetMqWaicaiConfig(string Key)
        {
            string result = "";
            IDictionary dict = ConfigurationManager.GetSection("RabbitWaicaiMQConfig") as IDictionary;
            if (dict[Key] != null && dict[Key].ToString().Length > 0)
            {
                result = dict[Key] as string;
            }
            else
            {
                throw new Exception("请按要求配置RabbitWaicaiMQConfig节点和相关信息");
            }
            return result;
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string GetMqConfig(string Key)
        {
            string result = "";
            IDictionary dict = ConfigurationManager.GetSection("RabbitMQBusinessConfig") as IDictionary;
            if (dict[Key] != null && dict[Key].ToString().Length > 0)
            {
                result = dict[Key] as string;
            }
            else
            {
                throw new Exception("请按要求配置RabbitMQ节点和相关信息");
            }
            return result;
        }
    }

    public class WaicaiMessage
    {
        /// <summary>
        /// 业务类型（添加用户）
        /// </summary>
        public string bussinessType { get; set; }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public string userAccount { get; set; }
    }

    /// <summary>
    /// 接收对象的实体
    /// </summary>
    public class ReceiveDataEntity
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string bussinessType { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string dataSource { get; set; }

        /// <summary>
        /// 交易编号
        /// </summary>
        public string bussinessId { get; set; }

        /// <summary>
        ///Json数据，根据业务类型来转换对应的实体
        /// </summary>
        public object bussinessForm { get; set; }
    }

    /// <summary>
    /// 回调消息实体
    /// </summary>
    public class CallBackMessageEntity
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string businessId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string businessType { get; set; }

        /// <summary>
        /// 在线业务ID
        /// </summary>
        public string onLineBusinessID { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public BussinessForm bussinessForm { get; set; }
    }

    public class BussinessForm
    {
        /// <summary>
        /// 成功状态
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BussinessType
    {
        预评估 = 0,
        预评估转正式 = 1,
        正式委托 = 2,
        附件上传 = 3,
        附件更新 = 4,
        信息补全 = 5,
        人工询价 = 6,
        信息反馈 = 7,
        业务终止 = 8,
        进度反馈 = 9,
        预约看房 = 10,
        实勘完成 = 11
    }
}
