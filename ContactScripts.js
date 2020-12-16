var Sdk = window.Sdk || {};

(
function () {

    this.formOnLoad = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var accountNameLookupArray = formContext.getAttribute("parentcustomerid").getValue();

        if (accountNameLookupArray && accountNameLookupArray[0] != null) {
            alert(`Guid: ${accountNameLookupArray[0].id} name: ${accountNameLookupArray[0].name} entityType: ${accountNameLookupArray[0].entityType}`)
        }

    }

    this.mobilePhoneOnChange = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var mobilePhone = formContext.getAttribute("mobilephone").getValue();

        var phoneno = /((\(\d{3}\) ?)|(\d{3}-))?\d{3}-d{4}/;

        if (!(phoneno.test(mobilePhone))) {

            formContext.getControl("mobilephone").setNotification("Please enter in US format", "wrongMobilePhoneMsg");
        } else {
            formContext.getControl("mobilephone").clearNotification("wrongMobilePhoneMsg");

        }
      
    }

    this.shippingMethodOnChange = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.getAttribute("address1_shippingmethodcode").getText() == "FedEx") {
            formContext.getControl("address1_freighttermscode").setDisabled(true);
        } else {
            formContext.getControl("address1_freighttermscode").setDisabled(false);
        }
    }

}).call(Sdk);