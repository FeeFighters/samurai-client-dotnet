## Overview

The Samurai API uses simple XML payloads transmitted over 256-bit encrypted <code class="post request">HTTPS POST</code> with Basic Authentication.  The Samurai .NET library handles all of the authentication & XML serialization.  It encapsulates the API calls as simple method calls, so you shouldn't need to think about the details of the actual API.



## Getting Started

To use the Samurai API, you'll need a <mark>Merchant Key</mark>, <mark>Merchant Password</mark> and a <mark>Processor Token</mark>.
[Sign up for an account](https://samurai.feefighters.com/users/sign_up) to get started.


### Samurai Credentials

Once you have these credentials, you can add them to a file in your project:

```dotnet
Samurai.Samurai.Options = new SamuraiOptions()
{
	MerchantKey = "[merchant_key]",
	MerchantPassword = "[merchant_password]",
	ProcessorToken = "[processor_token]"
};
```

If you're using ASP.NET MVC Framework put this code in Global.aspx file into 
Application_Start() method so this method should be something like this (for MVC 3):

```dotnet
protected void Application_Start()
{
	AreaRegistration.RegisterAllAreas();

	RegisterGlobalFilters(GlobalFilters.Filters);
	RegisterRoutes(RouteTable.Routes);

	Samurai.Samurai.Options = new SamuraiOptions()
	{
  	MerchantKey = "[merchant_key]",
  	MerchantPassword = "[merchant_password]",
  	ProcessorToken = "[processor_token]"
	};
}
```

The ProcessorToken property is optional. If you set it,
`Processor.TheProcessor` will return the processor with this token. You
can always call `new Processor("an_arbitrary_processor_token")` to
retrieve any of your processors.



## Payment Methods

Each time a user stores their billing information in the Samurai system, we call it a <mark>Payment Method</mark>.

Our [transparent redirect](https://samurai.feefighters.com/transparent-redirect) uses a simple HTML form on your website that submits your user’s billing information directly to us over SSL so that you never have to worry about handling credit card data yourself. We’ll quickly store the sensitive information, tokenize it for you and return the user to a page on your website of your choice with the new <mark>Payment Method Token</mark>.

From that point forward, you always refer to the Payment Method Token anytime you’d like to use that billing information for anything.

### Creating a Payment Method (via Transparent Redirect)

Setting up a transparent redirect form extremely simple. Here’s an example of the form that will be added to your view.  The style and labels are yours to configure however you like.

```html
<form action="https://api.samurai.feefighters.com/v1/payment_methods" method="POST">
  <fieldset>
    <input name="redirect_url" type="hidden" value="http://yourdomain.com/anywhere" />
    <input name="merchant_key" type="hidden" value="[merchant_key]" />
    <input name="sandbox" type="hidden" value="true" />

    <!-- Before populating the ‘custom’ parameter, remember to escape reserved characters
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

    <label for="credit_card_card_number">Card Number</label>
    <input id="credit_card_card_number" name="credit_card[card_number]" type="text" />

    <label for="credit_card_cvv">Security Code</label>
    <input id="credit_card_cvv" name="credit_card[cvv]" type="text" />

    <label for="credit_card_month">Expires on</label>
    <input id="credit_card_month" name="credit_card[expiry_month]" type="text" />
    <input id="credit_card_year" name="credit_card[expiry_year]" type="text" />

    <button type='submit'>Submit Payment</button>
  </fieldset>
</form>
```

When we return the user to the URL that you specified in the `redirect_url` field, we’ll append the new Payment Method Token to the query string. You should save the Payment Method Token and use it from this point forward.

The following fields are required by Samurai, and submitting a Payment Method without them will trigger an error: `card_number`, `cvv`, `expiry_month`, `expiry_year`. However, in practice, most Processors will also require `first_name`, `last_name`, and the address fields as well. In that case, leaving these fields blank will trigger an error from the Processor when the Payment Method is used to create a transaction.

The `sandbox` field is a boolean that specifies whether the payment method is intended to be used with a <mark>Sandbox Processor</mark>.  Sandbox processors can only process transactions with sandbox Payment Methods, and vice versa.  You may omit this field in production, the default value is "false".

### Update a Payment Method (via Transparent Redirect)

Updating a payment method is very similar to creating a new one.  (in fact, it is so similar that we've considered calling it "regenerating" instead)

To update a payment method, we add an additional hidden input `payment_method_token` for the desired payment method to be regenerated via the transparent redirect. Key fields are highlighted in bold text, the style and labels are yours to configure however you like. All credit card related fields are optional along with the custom and sandbox fields.

The semantics around regenerating a Payment Method using an existing `payment_method_token` are:

* If `field` does not exist, use old Payment Method value
* If `field` exists, but is invalid, use old Payment Method value
* If `field` exists, and is valid (or has no validations), replace old Payment Method value
  * __The only fields that are required by Samurai are `card_number, cvv, expiry_month, expiry_year`. For the other fields, blank values will be considered valid.__

This is designed to allow you to load the old Payment Method data into your form fields, including the sanitized card number (`last_four_digits`) and cvv values. Then, when the user changes any of the fields and submits the form it should regenerate the new Payment Method keeping the old data intact and updating only the fresh data.

```html
<form action="https://api.samurai.feefighters.com/v1/payment_methods" method="POST">
  <fieldset>
    <input name="redirect_url" type="hidden" value="http://yourdomain.com/anywhere" />
    <input name="merchant_key" type="hidden" value="[merchant_key]" />
    <input name="payment_method_token" type="hidden"; value="[Payment Method Token]" />
    <input name="sandbox" type="hidden" value="true" />

    ...

    <button type='submit'>Submit Payment</button>
  </fieldset>
</form>
```


### Fetching a Payment Method

Since the transparent redirect form submits directly to Samurai, you don’t get to see the data that the user entered until you read it from us.  This way, you can see if the user made any input errors and ask them to resubmit the transparent redirect form if necessary.

We’ll only send you non-sensitive data, so you will no be able to pre-populate the sensitive fields (card number and cvv) in the resubmission form.

```dotnet
PaymentMethod paymentMethod = PaymentMethod.Fetch(paymentMethodToken)
PaymentMethod paymentMethod.IsSensitiveDataValid  // gets true if the credit_card[card_number] passed checksum
											                            // and the cvv (if included) is a number of 3 or 4 digits
```

**NOTE:** Samurai will not validate any non-sensitive data so it is up to your
application to perform any additional validation on the payment_method.


### Retaining a Payment Method

Once you have determined that you’d like to keep a Payment Method in the Samurai vault, you must tell us to retain it.  If you don’t explicitly issue a retain command, we will delete the Payment Method within 48 hours in order to comply with PCI DSS requirement 3.1, which states:

> "3.1  Keep cardholder data storage to a minimum. Develop a data retention and disposal policy. Limit storage amount and retention time to that which is required for business, legal, and/or regulatory purposes, as documented in the data retention policy."

However, if you perform a purchase or authorize transaction with a Payment Method, it will be automatically retained for future use.

```dotnet
paymentMethod.Retain()
```

### Redacting a Payment Method

It’s important that you redact payment methods whenever you know you won’t need them anymore.  Typically this is after the credit card’s expiration date or when your user has supplied you with a different card to use.

```dotnet
paymentMethod.Redact()
```



## Processing Payments (Simple)

When you’re ready to process a payment, the simplest way to do so is with the `purchase` method.

```dotnet
Processor processor = Processor.TheProcessor // if you set Samurai.Options.ProcessorToken
Transaction purchase = processor.Purchase(paymentMethodToken, amount)
string purchaseReferenceId = purchase.ReferenceId // save this value, you can find the transaction with it later
```

`processor.Purchase` accepts optional arguments:

```dotnet
Transaction purchase = processor.Purchase(paymentMethodToken, amount, descriptor, custom, customer_reference, billing_reference)
```

* `descriptor`: descriptor for the transaction
* `custom`: custom data, this data does not get passed to the processor, it is stored within api.samurai.feefighters.com only
* `customer_reference`: an identifier for the customer, this will appear in the processor if supported
* `billing_reference`: an identifier for the purchase, this will appear in the processor if supported



## Processing Payments (Complex)

In some cases, a simple purchase isn’t flexible enough.  The alternative is to do an Authorize first, then a Capture if you want to process a previously authorized transaction or a Void if you want to cancel it.  Be sure to save the `transaction_token` that is returned to you from an authorization because you’ll need it to capture or void the transaction.

### Authorize

An Authorize doesn’t charge your user’s credit card.  It only reserves the transaction amount and it has the added benefit of telling you if your processor thinks that the transaction will succeed whenever you Capture it.

```dotnet
Transaction authorization = processor.Authorize(paymentMethodToken, amount)
string authorizationReferenceId = authorization.ReferenceId // save this value, you can find the transaction with it later
```

See the [purchase transaction](#processing-payments-simple) documentation for details on the optional data parameters.

### Capture

You can only execute a capture on a transaction that has previously been authorized.  You’ll need the Transaction Token value from your Authorize command to construct the URL to use for capturing it.

```dotnet
Transaction authorization = Transaction.Fetch(authorizationReferenceId) // get the authorization created previously
Transaction capture = authorization.Capture() // captures the full amount of the authorization
```

### Void

You can only execute a void on a transaction that has previously been captured or purchased.  You’ll need the Transaction Token value from your Authorize command to construct the URL to use for voiding it.

**A transaction can only be voided if it has not settled yet.** Settlement typically takes 24 hours, but it depends on the processor connection you are using. For this reason, it is often convenient to use the [Reverse](#reverse) instead.

```dotnet
Transaction transaction = Transaction.Fetch(purchaseReferenceId) // gets the purchase created before previously
Transaction void_transaction = transaction.Void() // voids the transaction
```

### Credit

You can only execute a credit on a transaction that has previously been captured or purchased.  You’ll need the Transaction Token value from your Authorize command to construct the URL to use for crediting it.

**A transaction can only be credited if it has settled.** Settlement typically takes 24 hours, but it depends on the processor connection you are using. For this reason, it is often convenient to use the [Reverse](#reverse) instead.

```dotnet
Transaction purchase = Transaction.Fetch(purchaseReferenceId)
Transaction credit = purchse.Credit() // credits the full amount of the original purchase
```

### Reverse

You can only execute a reverse on a transaction that has previously been captured or purchased.  You’ll need the Transaction Token value from your Authorize command to construct the URL to use for reversing it.

*A reverse is equivalent to a void, followed by a full-value credit if the void is unsuccessful.*

_Documentation coming soon._


## Fetching a Transaction

Each time you use one of the transaction processing functions (purchase, authorize, capture, void, credit) you are given a `reference_id` that uniquely identifies the transaction for reporting purposes.  If you want to retrieve transaction data, you can use this fetch method on the `reference_id`.

```dotnet
Transaction transaction = Transaction.Fetch(referenceId) // get the authorization created previously
```



## Server-to-Server Payment Method API

We don't typically recommend using our server-to-server API for creating/updating Payment Methods, because it requires credit card data to pass through your server and exposes you to a much greater PCI compliance & risk liability.

However, there are situations where using the server-to-server API is appropriate, such as integrating a server that is already PCI-secure with Samurai or if you need to perform complex transactions and don't mind bearing the burden of greater compliance and risk.


### Creating a Payment Method (S2S)

_Documentation coming soon._

### Updating a Payment Method (S2S)

Any payment method that has been retained (either explicitly via the retain function or implicitly via purchase or authorize) cannot be updated any longer.

```dotnet
PaymentMethod paymentMethod = PaymentMethod.Fetch(paymentMethodToken);
paymentMethod.FirstName = 'Dave';
paymentMethod.Update();
```


## Error Messages

The PaymentMethod and Transaction classes both load errors automatically from the Samurai API, adding them to the `ProcessorResponse` class.  The messages can be queried via `Messages`, which will return an array of `Message` objects.

```dotnet
ProcessorResponse processorResponse = transaction.getProcessorResponse();
List<Message> messages = processorResponse.getMessages();
```
