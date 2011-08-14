Samurai
=======

If you are an online merchant and using samurai.feefighers.com, this library will
make your life easy. Integrate with the samuari.feefighters.com portal and 
process transactions.

Installation
------------

Just download source  code and compile Samurai project then add Samurai.dll to your project references.
If you want to move Samurai.dll also move RestSharp.dll and Newtonsoft.Json.dll.

Configuration
-------------

Set the Samurai.Samurai.Options before you'll use this API.

	Samurai.Samurai.Options = new SamuraiOptions()
	{
		MerchantKey = "your_merchant_key",
		MerchantPassword = "your_merchant_password",
		ProcessorToken = "your_default_processor_token"
	};

If you're using ASP.NET MVC Framework put this code in Global.aspx file into 
Application_Start() method so this method should be something like this (for MVC 3):

	protected void Application_Start()
	{
		AreaRegistration.RegisterAllAreas();

		RegisterGlobalFilters(GlobalFilters.Filters);
		RegisterRoutes(RouteTable.Routes);

		Samurai.Samurai.Options = new SamuraiOptions()
		{
			MerchantKey = "your_merchant_key",
			MerchantPassword = "your_merchant_password",
			ProcessorToken = "your_default_processor_token"
		};
	}

The ProcessorToken property is optional. If you set it,
`Samurai.Processor.TheProcessor` will return the processor with this token. You
can always call `new Samurai.Processor("an_arbitrary_processor_token")` to
retrieve any of your processors.

Payment Methods
---------------

A Payment Method is created each time a user stores their billing information
in Samurai. 

### Creating Payment Methods

To let your customers create a Payment Method, place a credit card
entry form on your site like the one below.

    <form action="https://samurai.feefighters.com/v1/payment_methods" method="POST">
      <fieldset>
        <input name="redirect_url" type="hidden" value="http://yourdomain.com/anywhere" />
        <input name="merchant_key" type="hidden" value="[Your Merchant Key]" />

        <!-- Before populating the ‘custom’ parameter, remember to escape reserved xml characters 
             like <, > and & into their safe counterparts like &lt;, &gt; and &amp; -->
        <input name="custom" type="hidden" value="Any value you want us to save with this payment method" />

        <label for="credit_card_first_name">First name</label>
        <input id="credit_card_first_name" name="credit_card[first_name]" type="text" />

        <label for="credit_card_last_name">Last name</label>
        <input id="credit_card_last_name" name="credit_card[last_name]" type="text" />

        <label for="credit_card_address_1">Address 1</label>
        <input id="credit_card_address_1" name="credit_card[address_1]" type="text" />

        <label for="credit_card_address_2">Address 2</label>
        <input id="credit_card_address_2" name="credit_card[address_2]" type="text" />

        <label for="credit_card_city">City</label>
        <input id="credit_card_city" name="credit_card[city]" type="text" />

        <label for="credit_card_state">State</label>
        <input id="credit_card_state" name="credit_card[state]" type="text" />

        <label for="credit_card_zip">Zip</label>
        <input id="credit_card_zip" name="credit_card[zip]" type="text" />

        <label for="credit_card_card_type">Card Type</label>
        <select id="credit_card_card_type" name="credit_card[card_type]">
          <option value="visa">Visa</option>
          <option value="master">MasterCard</option>
        </select>

        <label for="credit_card_card_number">Card Number</label>
        <input id="credit_card_card_number" name="credit_card[card_number]" type="text" />

        <label for="credit_card_verification_value">Security Code</label>
        <input id="credit_card_verification_value" name="credit_card[cvv]" type="text" />

        <label for="credit_card_month">Expires on</label>
        <input id="credit_card_month" name="credit_card[expiry_month]" type="text" />
        <input id="credit_card_year" name="credit_card[expiry_year]" type="text" />

        <button type="submit">Submit Payment</button>
      </fieldset>
    </form>

After the form submits to Samurai, the user's browser will be returned to the 
URL that you specify in the redirect_url field, with an additional query 
parameter containing the `payment_method_token`. You should save the 
`payment_method_token` and use it from this point forward.

### Fetching a Payment Method

To retrieve the payment method and ensure that the sensitive data is valid: 

    PaymentMethod paymentMethod = PaymentMethod.Fetch(paymentMethodToken)
    PaymentMethod paymentMethod.IsSensitiveDataValid // gets true if the credit_card[card_number] passed checksum
													 // and the cvv (if included) is a number of 3 or 4 digits

**NB:** Samurai will not validate any non-sensitive data so it is up to your 
application to perform any additional validation on the payment_method.

### Updating Payment Methods

You can update the payment method by directly setting its properties and then
calling its Update() method:

	PaymentMethod paymentMethod = PaymentMethod.Fetch(paymentMethodToken);
	paymentMethod.FirstName = 'Dave';
	paymentMethod.Update();

### Retaining and Redacting Payment Methods

Unless you create a transaction on a payment method right away, that payment
method will be purged from Samurai. If you want to hang on to a payment method
for a while before making an authorization or purchase on it, you must retain it:

    paymentMethod.Retain()

If you are finished with a payment method that you have either previously retained
or done one or more transactions with, you may redact the payment method. This 
removes any sensitive information from Samurai related to the payment method, 
but it still keeps the transaction data for reference. No further transactions
can be processed on a redacted payment method. 

    paymentMethod.Redact()

Processing Transactions
-----------------------

Your application needs to be prepared to track several identifiers. The payment_method_token
identifies a payment method stored in Samurai. Each transaction processed
has a transaction_token that identifies a group of transactions (initiated with
a purchase or authorization) and a reference_id that identifies the specific
transaction. 

### Purchases and Authorizations

When you want to start to process a new purchase or authorization on a payment 
method, Samurai needs to know which of your processors you want to use. You can
initiate a purchase (if your processor supports it) or an authorization against
a processor by:

    Processor processor = Processor.TheProcessor // if you set Samurai.Options.ProcessorToken
    Processor processor = new Processor("a_processor_token") // if you have multiple processors
    Transaction purchase = processor.Purchase(paymentMethodToken, amount)
    string purchaseReferenceId = purchase.ReferenceId // save this value, you can find the transaction with it later
    
An authorization is created the same way: 
    
    Transaction authorization = processor.Authorize(paymentMethodToken, amount)
    string authorizationReferenceId = authorization.ReferenceId // save this value, you can find the transaction with it later

You can specify another option parameters for either transaction type:

* descriptor: a string description of the charge
* custom: a custom value that Samurai will store but not forward to the processor
* customerReference: a string that identifies the customer to your application
* billingReference: a string reference for the transaction

### Capturing an Authorization

An authorization only puts a hold on the funds that you specified. It won't 
capture the money. You'll need to call capture on the authorization to do this.

    Transaction authorization = Transaction.Fetch(authorizationReferenceId) // get the authorization created previously
    Transaction capture = authorization.Capture() // captures the full amount of the authorization

### Voiding a Transaction

A transaction that was recently created can be voided, if is has not been 
settled. A transaction that has settled has already deposited funds into your
merchant account. 

    Transaction transaction = Transaction.Fetch(purchaseReferenceId) // gets the purchase created before previously
    Transaction void_transaction = transaction.Void() // voids the transaction

### Crediting a Transaction

Once a captured authorization or purchase has settled, you need to credit the 
transaction if you want to reverse a charge. 

    Transaction purchase = Transaction.Fetch(purchaseReferenceId)
    Transaction credit = purchse.Credit() // credits the full amount of the original purchase
