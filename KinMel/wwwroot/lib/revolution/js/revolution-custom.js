
(function($){
  "use strict";

/*************************
      Slider 1
*************************/
	var tpj=jQuery;
  tpj(document).ready(function() {
       var revapi3;     
        if(tpj("#rev_slider_3_1").revolution == undefined){
          revslider_showDoubleJqueryError("#rev_slider_3_1");
        }else{
          revapi3 = tpj("#rev_slider_3_1").show().revolution({
            sliderType:"standard",
            sliderLayout:"fullwidth",
            dottedOverlay:"none",
            delay:9000,            
            navigation: {
              keyboardNavigation:"off",
              keyboard_direction: "horizontal",
              mouseScrollNavigation:"off",
              mouseScrollReverse:"default",
              onHoverStop:"off",
              touch:{
                touchenabled:"on",
                swipe_threshold: 75,
                swipe_min_touches: 1,
                swipe_direction: "horizontal",
                drag_block_vertical: false
              }
              ,
              bullets: {
              enable:true,
              hide_onmobile:false,
              style:"ares",
              hide_onleave:false,
              direction:"horizontal",
              h_align:"right",
              v_align:"bottom",
              h_offset:40,
              v_offset:40,
              space:15,
                tmp:'<span class="tp-bullet-title">{{title}}</span>'
              }
            },
            visibilityLevels:[1240,1024,778,480],
            gridwidth:1170,
            gridheight:990,
            lazyType:"none",
            shadow:0,
            spinner:"spinner2",
            stopLoop:"off",
            stopAfterLoops:-1,
            stopAtSlide:-1,
            shuffle:"off",
            autoHeight:"off",
            disableProgressBar:"on",
            hideThumbsOnMobile:"off",
            hideSliderAtLimit:0,
            hideCaptionAtLimit:0,
            hideAllCaptionAtLilmit:0,
            debugMode:false,
            fallbacks: {
              simplifyAll:"off",
              nextSlideOnWindowFocus:"off",
              disableFocusListener:false,
            }
          });
        }
      
/*************************
      Slider 2
*************************/
 
  var revapi7;   
    if(tpj("#rev_slider_7_1").revolution == undefined){
     revslider_showDoubleJqueryError("#rev_slider_7_1");
    }else{
     revapi7 = tpj("#rev_slider_7_1").show().revolution({
      sliderType:"standard",
			jsFileLocation:"//localhost/revslider-standalone/revslider/public/assets/js/",
      sliderLayout:"fullwidth",
      dottedOverlay:"none",
      delay:9000,
      navigation: {
       keyboardNavigation:"off",
       keyboard_direction: "horizontal",
       mouseScrollNavigation:"off",
       mouseScrollReverse:"default",
       onHoverStop:"off",
       touch:{
        touchenabled:"on",
        swipe_threshold: 75,
        swipe_min_touches: 1,
        swipe_direction: "horizontal",
        drag_block_vertical: false
       }
       ,
       arrows: {
        style:"persephone",
        enable:true,
        hide_onmobile:false,
        hide_onleave:false,
        tmp:'',
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
      visibilityLevels:[1240,1024,778,480],
      gridwidth:1920,
      gridheight:900,
      lazyType:"none",
      parallax: {
       type:"mouse",
       origo:"enterpoint",
       speed:400,
       levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,55]
       },
      shadow:0,
      spinner:"spinner2",
      stopLoop:"off",
      stopAfterLoops:-1,
      stopAtSlide:-1,
      shuffle:"off",
      autoHeight:"off",
      disableProgressBar:"on",
      hideThumbsOnMobile:"off",
      hideSliderAtLimit:0,
      hideCaptionAtLimit:0,
      hideAllCaptionAtLilmit:0,
      debugMode:false,
      fallbacks: {
       simplifyAll:"off",
       nextSlideOnWindowFocus:"off",
       disableFocusListener:false,
      }
    });
  }

      
/*************************
      Slider 3
*************************/

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

      
/*************************
      Slider 4
*************************/

      	var revapi20;
				if(tpj("#rev_slider_20_1").revolution == undefined){
          revslider_showDoubleJqueryError("#rev_slider_20_1");
        }else{
          revapi20 = tpj("#rev_slider_20_1").show().revolution({
            sliderType:"standard",
            sliderLayout:"fullwidth",
            dottedOverlay:"none",
            delay:9000,
            navigation: {
              keyboardNavigation:"off",
              keyboard_direction: "horizontal",
              mouseScrollNavigation:"off",
              mouseScrollReverse:"default",
              onHoverStop:"off",
              touch:{
                touchenabled:"on",
                swipe_threshold: 75,
                swipe_min_touches: 1,
                swipe_direction: "horizontal",
                drag_block_vertical: false
              }
              ,
              arrows: {
                style:"hermes",
                enable:true,
                hide_onmobile:false,
                hide_onleave:false,
                tmp:'<div class="tp-arr-allwrapper">  <div class="tp-arr-imgholder"></div>  <div class="tp-arr-titleholder">{{title}}</div> </div>',
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
            gridheight:[700,768,600,500],
            lazyType:"none",
            shadow:0,
            spinner:"spinner2",
            stopLoop:"off",
            stopAfterLoops:-1,
            stopAtSlide:-1,
            shuffle:"off",
            autoHeight:"off",
            disableProgressBar:"on",
            hideThumbsOnMobile:"off",
            hideSliderAtLimit:0,
            hideCaptionAtLimit:0,
            hideAllCaptionAtLilmit:0,
            debugMode:false,
            fallbacks: {
              simplifyAll:"off",
              nextSlideOnWindowFocus:"off",
              disableFocusListener:false,
            }
          });
        }

      
/*************************
      Slider 5
*************************/
        
     		var revapi21;
				if(tpj("#rev_slider_21_1").revolution == undefined){
          revslider_showDoubleJqueryError("#rev_slider_21_1");
        }else{
          revapi21 = tpj("#rev_slider_21_1").show().revolution({
            sliderType:"standard",
            sliderLayout:"fullwidth",
            dottedOverlay:"none",
            delay:9000,
            navigation: {
              keyboardNavigation:"off",
              keyboard_direction: "horizontal",
              mouseScrollNavigation:"off",
              mouseScrollReverse:"default",
              onHoverStop:"off",
              touch:{
                touchenabled:"on",
                swipe_threshold: 75,
                swipe_min_touches: 1,
                swipe_direction: "horizontal",
                drag_block_vertical: false
              }
              ,
              arrows: {
                style:"hermes",
                enable:true,
                hide_onmobile:false,
                hide_onleave:false,
                tmp:'<div class="tp-arr-allwrapper">  <div class="tp-arr-imgholder"></div>  <div class="tp-arr-titleholder">{{title}}</div> </div>',
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
            gridwidth:[1170,1024,778,480],
            gridheight:[1000,768,600,500],
            lazyType:"none",
            shadow:0,
            spinner:"spinner2",
            stopLoop:"off",
            stopAfterLoops:-1,
            stopAtSlide:-1,
            shuffle:"off",
            autoHeight:"off",
            disableProgressBar:"on",
            hideThumbsOnMobile:"off",
            hideSliderAtLimit:0,
            hideCaptionAtLimit:0,
            hideAllCaptionAtLilmit:0,
            debugMode:false,
            fallbacks: {
              simplifyAll:"off",
              nextSlideOnWindowFocus:"off",
              disableFocusListener:false,
            }
          });
        }
      });

    })(jQuery);