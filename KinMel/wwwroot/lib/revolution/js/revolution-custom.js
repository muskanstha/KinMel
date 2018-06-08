
(function($){
  "use strict";
    var tpj = jQuery;
    tpj(document).ready(function () {

      var revapi19;
			if(tpj("#rev_slider_19_1").revolution == undefined){
          revslider_showDoubleJqueryError("#rev_slider_19_1");
        }else{
          revapi19 = tpj("#rev_slider_19_1").show().revolution({
            sliderType:"standard",
            sliderLayout: "auto",
            dottedOverlay:"none",
            delay:9000,
            navigation: {
              keyboardNavigation:"off",
              keyboard_direction: "horizontal",
              mouseScrollNavigation:"off",
              mouseScrollReverse:"default",
              onHoverStop:"on",
              touch:{
                touchenabled:"on",
                swipe_threshold: 75,
                swipe_min_touches: 1,
                swipe_direction: "horizontal",
                drag_block_vertical: false
              }
              ,
              arrows: {
                style:"zeus",
                enable:true,
                hide_onmobile:false,
                hide_onleave:false,
                tmp:'<div class="tp-title-wrap">    <div class="tp-arr-imgholder"></div> </div>',
                left: {
                  h_align:"left",
                  v_align:"center",
                  h_offset:20,
                                    v_offset:0
                },
                right: {
                  h_align:"right",
                  v_align:"center",
                  h_offset:20,
                                    v_offset:0
                }
              }
            },
            responsiveLevels:[1240,1024,778,480],
            visibilityLevels:[1240,1024,778,480],
            gridwidth:[1270,1024,778,480],
            gridheight:[500,440,350,280],
            lazyType:"none",
            shadow:0,
            spinner:"spinner2",
            stopLoop:"off",
            stopAfterLoops:-1,
            stopAtSlide:-1,
              shuffle: "off",
            autoHeight:"on",
            disableProgressBar:"off",
            hideThumbsOnMobile:"off",
            hideSliderAtLimit:0,
            hideCaptionAtLimit:0,
            hideAllCaptionAtLilmit:0,
            debugMode:false,
            fallbacks: {
              simplifyAll:"off",
              nextSlideOnWindowFocus:"off",
              disableFocusListener:false
            }
          });
        }
    });
})(jQuery);