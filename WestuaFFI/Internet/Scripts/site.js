$(function () {

    $("#navigation a:first").click();

    $(".products li a").live("mouseenter", function() {
         $(this).animate({
                        'margin-left': 20,
                    }, 200, function() {});
        $("#productImg").attr("src", "/Products/GetImage?productId=" + $(this).attr("id"));        
    }); 
    $(".products li a").live("mouseleave", function(){
        $(this).animate({
                        'margin-left': 0,
                    }, 200, function() {});
    });
});

function PreSubmitPayment(amount, userName) {
    var form = $("#paymentForm");
    $.post("/Partners/AddPayment",
        { amount: amount, userName: userName },
        function(data) {
            if (data != -1) {
                form.find("input[name=order_id]").val(data);
                form.submit();
            }
        }
    );
}