![.NET Core](https://github.com/aimenux/FireAndForgetDemo/workflows/.NET%20Core/badge.svg)
# FireAndForgetDemo
```
Demo of using a fire & forget tasks
```

> `Definition` : We talk about “fire & forget” pattern when a process call another thread and continue the process flow, without waiting for a response from the called thread.
>> The called thread (i.e `fire/forget` task) should always terminate successfully without impacting performance or exiting the process flow. it may swallow or log exceptions.

> Multiple examples are implemented :
>
> **(1)** Example1 : both `awaitable` & `fire/forget` tasks are independent and successful
>> `Example1` will terminate successfully and will spend as much time as `awaitable` task
>> `fire/forget` task will terminate successfully without errors logging
>
> **(2)** Example2 : both `awaitable` & `fire/forget` tasks are independent, `fire/forget` will throw an exception
>> `Example2` will terminate successfully and will spend as much time as `awaitable` task
>> `fire/forget` task will terminate successfully with errors logging
>
> **(2)** Example3 : `awaitable` & `fire/forget` tasks are dependent, `fire/forget` may or not throw an exception
>> `Example3` will terminate successfully and will spend as much time as `awaitable` task
>> `fire/forget` task will terminate successfully without errors logging when FoundDocumentsOnUnstableStorage is set to true
>> `fire/forget` task will terminate successfully with errors logging when FoundDocumentsOnUnstableStorage is set to false

**`Tools`** : vs19, net core 3.1