$(document).ready(function () {
    //on load first populate drp then attach event handler to change event
    $('#lblResult').hide();
    GetXMLRestaurants();
    $('#drpRestaurant').change(function (ev) {
        $('#lblResult').hide();
        if (this.value != "-1") {
            // get restaurant data by name!
            GetXMLRestaurantByName(this.value);
            //also bind save even here!?
        }
        else {
            EmptyInputs();
        }
    });
    $('#btnSave').click(function (ev) {
        if ($('#drpRestaurant').val() != "-1") {
            SaveXML();
        }
    });
});
function GetXMLRestaurants() {
    var sendRequestTo = window.location.origin + GET_RESTAURANTS_URL;
    $.ajax({
        type: "GET",
        url: sendRequestTo,
        success: PopulateSelectRestaurant,
        error: AlertError
    });
}
function GetXMLRestaurantByName(name) {
    var sendRequestTo = window.location.origin + GET_RESTAURANT_BYNAME + "?id=" + name;
    $.ajax({
        type: "GET",
        url: sendRequestTo,
        success: PopulateInputs,
        error: AlertError
    });

}
function SaveXML() {
    var sendRequestTo = window.location.origin + POST_CHANGES_URL;
    var data = GetFormData();
    $.ajax({
        type: "POST",
        url: sendRequestTo,
        data: data,
        success: DisplaySuccess,
        error: AlertError
    });

}
function DisplaySuccess(data) {
    var message = data.message;
    $('#lblResult').show().text(message);
}
function PopulateSelectRestaurant(data) {
    var restaurants = data;
    restaurants.forEach(function (element) {
        AddSelection('#drpRestaurant', element.name, element.name);
    });
}
function PopulateInputs(data) {
    var restaurant = data;
    var address = restaurant.address.street + " " + restaurant.address.city + " " + restaurant.address.postalCode;
    var summary = restaurant.summary;
    var rating = restaurant.rating;
    $('#txtAddress').val(address);
    $('#txtSummary').val(summary);
    $('#drpRating').val(rating.toString());
}
function EmptyInputs() {
    $('#txtAddress').val("");
    $('#txtSummary').val("");
    $('#drpRating').val("1");
}
function GetFormData() {
    var restaurantReview = { Name: $('#drpRestaurant').val(), Rating: parseInt($('#drpRating').val()), Summary: $('#txtSummary').val(), Address: { City: "", Street: "", PostalCode: "", Province: "" } }
    return restaurantReview;
}
function AlertError(xhr, status, error) {
    alert("xhr: " + xhr + "status: " + status + "error: " + error);
}
function AddSelection(selector, val, txt) {
    $(selector).append($('<option>', {
        value: val,
        text: txt
    }))
}
var GET_RESTAURANTS_URL = "/restaurantreview/GetRestaurantNames";
var GET_RESTAURANT_BYNAME = "/restaurantreview/GetRestaurantByName";
var POST_CHANGES_URL = "/restaurantreview/SaveRestaurant"