ZBEngine
========

## 环境要求

	.net core 2.0/asp.net core 2.0
	vs2017 15.4
    Unity 5.6.4

## 什么是ZBEngine?

	1，一款基于 .net core 的开源游戏服务器引擎，
	2，使用.net core 可以方便的跨Windows,Linux平台
    3，此服务器还有一个优点是API尽量与Unity的API保持一致，方便一个人仅仅使用一门语言【C#】就可以同时快速开发前后端。
    4，提供了一个Unity的Client Demo。
    
## ZBEngine的目标

	1，实现TCP通信，支持Json,MsgPack,Protobuf的消息序列化反序列化
	2，简化多线程，让逻辑线程单独独立成一个携程，在携程里面可以进行简单的异步无锁操作
	3，实现数据库的访问，缓存的逻辑
	4，实现HTTP通信，同样HTTP Post模式下支持 Json,Msgpack,Protobuf的消息序列化和反序列化
	5，实现服务器间的通信
	6，基于Web的方式实现服务器状态，性能的监控

## 联系

QQ交流群：642442400