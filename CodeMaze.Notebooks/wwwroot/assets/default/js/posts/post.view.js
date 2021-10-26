$(function () {
    //< !--Magnific Popup(.js - gallery class is initialized in Helpers.magnific())-- >
    Mazek.helpers(['magnific']);

    jQuery("article img").lazyload({
        effect: "fadeIn"
    });

    document.addEventListener('DOMContentLoaded', (event) => {
        document.querySelectorAll('pre code').forEach((block) => {
            hljs.highlightBlock(block);
        });
    });
});