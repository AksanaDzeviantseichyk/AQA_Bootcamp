# Task 5 Optional - NUnit

Your task is to write unit tests using NUnit library for public methods CreateOrder(), AddItem(), CancelItem() and Checkout () of 'OrderProvider'  class.

You need to verify:
- adding and canceling items
- all BillExternal and BillExternalItem fields after checkout
- logic of discounting by time (For example we have 2 products. First product (Drink) has discount by time, second product (Main) has not. We add to order 1 piece of Drink while discount is active and 1 piece of Main. Few minutes later when discount is not active we try to add 1 Drink and 1 Main more. Expected result after checkout – both Mains are calculated without discount, the only 1 Drink calculated with discount).
- logic of service charge (if it is 0%/10%...)
- feature integrations (like add 2 items while discount is active, remove 1 when discount is over and add 2 more. That all together with some service charge).
 - check Tests assembly for example (examples are not optimized and can be refactored).

Here is a solution with RestaurantErp (Enterprise resource planning) Application:

https://github.com/TiAutoBootcamp/RestaurantErp

Solution projects:
·	'Restaurant.Models' project contains all models, classes that represent some entitites in our system.
·	'RestaurantErp.Core' project contains main logic. 
·	‘RestaurantErp.Tests’ project contains some examples of already implemented unit tests. 
·	‘RestaurantErp.WebApi’ project contains simple Server side http-based application. 

‘WebApi’ project is no more than simple proxy application, because whole application logic described in ‘Core’ assembly. It is a reason why this functionality should be covered not by api tests, but by unit tests.

IOrderProvider (implemented by ‘OrderProvider’ class) is main interface in this application that is responsible for order creating, editing, checkout operation.

It has such functions:
·	CreateOrder() – creates new order and returns orderId (Guid).
·	AddItem() – adds information about new item to existing order based on parameter ‘OrderItemRequest’.
·	CancelItem () – remove information about already added to order item based on parameter ‘OrderItemRequest’.
·	Checkout() – returns ‘BillExternal’ entity for requested orderId. 

‘OrderProvider’ class has dependencies:
·	‘IPriceStorage’ interface (implemented by ‘ProductProvider’ class) – this interface is responsible about providing information about product price.
·	‘IDiscountProvider’ interfaces (implemented by ‘DiscountByTimeProvider’ class) – this interface is responsible for calculating discount based on ‘Order’ info.
·	‘IDiscountCalculator’ interface (implemented by ‘DiscountCalculator’ class) - this interface is responsible for applying already calculated discount (by ‘IDiscountProvider’ interface) to existing Bill.
DiscountByTimeProvider is class that implement logic of discounting by time
·	‘ITimeHelper’ interface (implemented by ‘TimeHelper’ class) - this interface is responsible for returning information about current time to use it as OrderingTime of Order item.
·	‘BillHelper’ class – responsible for converting ‘Bill’ entity (our internal class that hide some inner logic) to ‘BillExternal’ entity (external class without sensitive information that can be returned later end customer). 
·	‘IServiceChargeProvider’ interface (implemented by ‘ServiceChargeProvider’ class) - this interface is responsible for applying service charge to existing Bill. ‘ServiceChargeProvider’ class has constructors parameter ‘ServiceChargeProviderSettings’ with field ‘ServiceRate’. ServiceRate is our service charge rate.

Checkout function of OrderProvider (or same endpoint) returns BillExternal entity with such fields:
BillExternal.OrderId = id of order that bill is based on
BillExternal.Amount = original amount without discount and service charge
BillExternal.Discount = discount value
BillExternal.AmountDiscounted = Bill.Amount - Bill.Discount
BillExternal.Service = service charge 
BillExternal.Total = Bill.AmountDiscounted - Bill.Service
BillExternal.Items[0].ProductName = name of product (for example Starter, Main, Drink...)
BillExternal.Items[0].PersonId = currently unused, need for possible logic of bill splitting
BillExternal.Items[0].Amount = product price
BillExternal.Items[0].Discount = discount value
BillExternal.Items[0].AmountDiscounted = Bill.Items[0].Amount - Bill.Items[0].Discount

Business requirements assumptions:
1.	Order item adding time calculated in class automatically. It means that due to delays (network etc.) we can meet problem when client made request at 19:00, but class received response few seconds later. To fix that we have ‘DiscountByTimeProvider’s constructor parameter 'EndDiscountDelaySeconds' - additional time after end of discount time period while discount still works.
2.	Application cancel all items in order of their ordering time (if we have few same products in order and we need to cancel the only one).
3.	In most countries we cannot set price of product less equal to zero (for example if discount is 100%), because then it will be gift not a sale (it regulated by another laws and cannot be provided by usual bill). So we add ‘DiscountCalculator’s constructor parameter ‘DiscountCalculatorSettings’ with field ‘MinimalProductPrice’.
4.	Technically I added contracts and logic of keeping information about which person from company (PersonId fields) ordered which item. But during checkout I didt add logic of splitting bill to few guests.
5.	Currently we have only discounting by time, but it is possible to pass different IDiscountProvider implementations to OrderProvider (because of collection parameter type) at the same time.
6.    We take service charge of discounted price. 