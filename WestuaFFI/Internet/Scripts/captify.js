$(function () {
    var width = 90;
    var height = 90;

    $('img.captify').each(function() {
            $(this).attr("width", width);
            $(this).attr("height", height);
    });

   $('img.captify').mouseenter(function () {
        var caption;
        if ($(this).parent().attr('class') != 'imgWrapper') {

            var wrapper = $('<div>', {
                class: 'imgWrapper',
                style: 'overflow: hidden; padding: 0px; font-size: 0.1px; width: '+ width +'px; height: '+height+'px; margin: 0px; border-style: none; border-width: 0px; border-color: rgb(51, 51, 51); position: relative;'
            });

            var captionText = $(this).attr("alt");

            caption = $('<div>', {
                class: 'imgCaption',
                style: 'background-color: white; ' +
                       'position: absolute; top:0; ' +
                       'left: -160px; z-index: 2; ' +
                       'width: '+ (width ) +'px; height: '+ (height - 30) +'px; ' +
                       'padding-top:'+ 30 +'px;' +
                       'text-align: center;',
                html: captionText 
            });

            $(this).wrap(wrapper);
            
            wrapper = $(this).parent();
            wrapper.append(caption).mouseleave(function() {
                    caption.animate({
                        left: -width,
                    }, 200, function() {});
                });
            
        } else {
            caption = $(this).siblings("div.imgCaption");
        }
        
        caption.animate({
            left: 0,
            }, 200, function() {});
    });
});