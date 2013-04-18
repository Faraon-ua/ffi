$(function () {

    $("#navigation a:first").click();

    $(".products li a").live("mouseenter", function() {
         $(this).animate({
                        'margin-left': 20,
                    }, 200, function() {});
    }); 
    $(".products li a").live("mouseleave", function(){
        $(this).animate({
                        'margin-left': 0,
                    }, 200, function() {});
    });
      
});