import asyncio

async def producer(queue):
    for i in range(5):
        await asyncio.sleep(1)
        item = f"Item {i}"
        await queue.put(item)
        print(f"Produced: {item}")

    # 发送一个特殊的值，表示生产结束
    await queue.put(None)

async def consumer(queue):
    while True:
        item = await queue.get()
        if item is None:
            # 收到特殊值表示生产结束
            break
        print(f"Consumed: {item}")

async def main():
    # 创建一个队列
    queue = asyncio.Queue()

    # 启动生产者和消费者协程
    producer_task = asyncio.create_task(producer(queue))
    consumer_task = asyncio.create_task(consumer(queue))
    # 等待生产者完成
    await producer_task

    # 等待消费者完成
    # await queue.join()
    # await queue.put(None)  # 发送一个特殊值，通知消费者生产结束
    # await consumer_task

# 运行主协程
asyncio.run(main())
