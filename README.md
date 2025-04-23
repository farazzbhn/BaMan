1. اندپوینت rpc/ با body زیر : \
` {"name":"F1","params":{"p1":"parameter 1 to pass to method F1"}} `


2.هندلر ریکوئست در تایپیک RPCMessage توسط redis pub/sub در سیستم publish می شود\
3. سرویس consumer ریجستر شده redis ( توسط پکیج کاستوم موجود در پروژه shared که برای پروژه نوشتم ) درخواست را دریافت کرده ، با ریفلکشن به متد تعیین شده پاس میدهد  و  پاسخ را با استفاده از c# channels در چنل RPCResult پاپلیش میکند\
4. سرویس listener رجیستر شده برای RPCResult ، پیام دریافتی را با استفاده از سرویس RPCService ذخیره میکند.\
5. سرویس RPCService با استفاده از semaphore به هندلر اطلاع میدهد که پاسخی برای RPC اولیه دریافت شده ، \
6. هندلر پاسخ  را به کلاینت باز میگرداند. ( یا خطای تایم اوت را )\
و برای run کردن ، redis رو روی port 6379 نیاز داریم\

کد های داخل shared بابت managedRedis و ManagedChannel رو هم برای پروژه نوشتم
