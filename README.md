پروژه با هدف Remote Procedure Call و برگردان return value ( یا خطا و یا timeout) به کلاینت
 
1. اندپوینت rpc/ با body زیر : \
` {"name":"F1","params":{"p1":"parameter 1 to pass to method F1"}} `


2.هندلر با دریافت ریکوئست پیامی از جنس  RPCMessage در تاپیک مرتبط توسط redis pub/sub در سیستم publish می کند و سپس منتظر دریافت پاسخ از سرویس RPCService میماند \
3. سرویس consumer رجیستر شده برای redis که به تاپیک RPCMessage سابسکرایب کرده  درخواست را دریافت میکند ، با ریفلکشن به متد تعیین شده پاس میدهد  و  پاسخ را با استفاده از c# channels در چنل RPCResult پاپلیش میکند\
4. سرویس reader برای چنل سی شارپی رجیستر شده برای RPCResult ، پیام دریافتی را با استفاده از سرویس RPCService ذخیره میکند.\
5. سرویس RPCService با استفاده از semaphore به هندلر اطلاع میدهد که پاسخی برای RPC اولیه دریافت شده ، \
6. هندلر پاسخ را به کلاینت باز میگرداند. ( یا خطای تایم اوت را )\
- برای run کردن ، redis رو روی port 6379 نیاز داریم\
- صرفا بابت show case کردن از دو تا مکانیزم pub/sub استفاده شده 
- کد های داخل shared بابت ManagedRedis و ManagedChannel رو هم برای پروژه نوشتم
