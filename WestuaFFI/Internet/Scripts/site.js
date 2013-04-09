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
});