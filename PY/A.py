import asyncio

async def foo():
    print("Start foo")
    await asyncio.sleep(1)
    print("End foo")

async def bar():
    print("Start bar")
    await asyncio.sleep(2)
    print("End bar")
    
async def main():
    task1 = asyncio.create_task(foo())
    task2 = asyncio.create_task(bar())
    await task1
    await task2
    # await asyncio.gather(task1, task2)
    # await asyncio.gather(foo(), bar())
    # await asyncio.wait([task  1, task2])-
    # done, pending = await asyncio.wait({task1, task2}, return_when=asyncio.FIRST_COMPLETED)

# 使用gather方法同时运行多个协程
asyncio.run(main())
