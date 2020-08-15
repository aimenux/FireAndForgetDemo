![.NET Core](https://github.com/aimenux/FireAndForgetDemo/workflows/.NET%20Core/badge.svg)
# FireAndForgetDemo
```
Demo of using a fire & forget tasks
```

> Multiple examples are implemented :
>
> **(1)** Example1 : both `awaitable` & `fire/forget` tasks are independent and successful
>> `Example1` will terminate successfully and will spend as much time as `awaitable` task
>
> **(2)** Example2 : both `awaitable` & `fire/forget` tasks are independent, `fire/forget` will throw an exception
>> `Example2` will terminate successfully and will spend as much time as `awaitable` task

**`Tools`** : vs19, net core 3.1