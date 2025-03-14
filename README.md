# Azure Event Hubs 示範專案

此專案展示如何使用 Azure Event Hubs 進行訊息的發送與接收。專案包含兩個主要組件：發送者（Sender）和接收者（Receiver）。

## EventHubsSender

發送者程式負責將事件批次發送至 Event Hub。

主要功能：
- 建立 EventHubProducerClient 連接至指定的 Event Hub
- 產生一個包含3個事件的批次
- 使用 UTF8 編碼將事件序列化
- 發送事件批次至 Event Hub
- 完成後自動釋放資源

關鍵程式碼：
```csharp
EventHubProducerClient producerClient = new EventHubProducerClient(connectionString);
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
```

## EventHubsReceiver

接收者程式負責從 Event Hub 接收並處理事件。

主要功能：
- 使用 Azure Blob Storage 作為檢查點儲存
- 建立 EventProcessorClient 處理來自 Event Hub 的事件
- 註冊事件處理和錯誤處理程序
- 接收並顯示事件內容
- 30秒後自動停止處理

關鍵程式碼：
```csharp
var processor = new EventProcessorClient(
    storageClient,
    EventHubConsumerClient.DefaultConsumerGroupName,
    connectionString,
    eventHubName);
```

## 事件處理流程

1. Sender 產生並發送事件批次
2. Event Hub 接收並儲存事件
3. Receiver 透過 EventProcessorClient 接收事件
4. 接收到的事件會被解碼並顯示在控制台

## 注意事項

- 需要有效的 Azure Event Hubs 連接字串
- 需要有效的 Azure Storage 帳戶用於 Receiver 的檢查點儲存
- 確保所有相關的 Azure 資源都已正確配置
