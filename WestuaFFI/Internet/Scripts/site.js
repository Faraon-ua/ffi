$(function () {
    $('img.captify').captify({
        // all of these options are... optional
        // ---
        // speed of the mouseover effect
        speedOver: 200,
        // speed of the mouseout effect
        speedOut: 200,
        // how long to delay the hiding of the caption after mouseout (ms)
        hideDelay: 0,
        // 'fade', 'slide', 'always-on'
        animation: 'slide',
        // text/html to be placed at the beginning of every caption
        prefix: '',
        // opacity of the caption on mouse over
        opacity: '1',
        // the name of the CSS class to apply to the caption box
        className: 'caption-bottom',
        // position of the caption (top or bottom)
        position: 'bottom',
        // caption span % of the image
        spanWidth: '100%'
    });

   
    
    $('img.captioning').mouseenter(function () {
        var caption;
        if ($(this).parent().attr('class') != 'imgWrapper') {

            var wrapper = $('<div>', {
                class: 'imgWrapper',
                style: 'overflow: hidden; padding: 0px; font-size: 0.1px; width: 160px; height: 160px; margin: 0px; border-style: none; border-width: 0px; border-color: rgb(51, 51, 51); position: relative;'
            });

            caption = $('<div>', {
                class: 'imgCaption',
                style: 'margin: 0; background-color: white; position: absolute; top:0; left: -160px; z-index: 2; border: 0px none; opacity: 1; width: 160px; height: 160px;',
                html: $(this).attr("alt")
            });

            $(this).wrap(wrapper);
            
            wrapper = $(this).parent();
            wrapper.append(caption).mouseleave(function() {
                caption.animate({
                    left: -160,
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