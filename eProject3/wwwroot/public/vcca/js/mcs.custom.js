jQuery(function() {
	/* Slider main */
	$('.slider-main .bxslider').bxSlider({
		controls: false,
		auto:true,
		speed:2000
	});
	$('.slider-main').css('height',$(window).height());
	$('.back-top').click(function() {
		$('html,body').animate({ scrollTop: 0 }, 600);
		return false;
	});
	/* Popup */
	$('.show-popup').click(function() {
		var id = $(this).attr('href');
		$(id).fadeIn();
		return false;
	});
	$('.close-popup').on('click',function() {
		$('.popup').fadeOut();
		return false;
	});
	$('.bg-transform').on('click',function() {
		$('.popup').fadeOut();
		return false;
	});
	/* Validate jQuery */
	$("#loginForm").validate({
	  rules: {
		login_name: "required",
		login_pass:"required"
	  },
	  messages: {
		login_name: "Please enter your username",
		login_pass: "Please enter your username"
	  }
	});
	/* JS VA */
	$('.content-about .slider-about .bxslider').bxSlider({
		auto:true,
		controls: false,
		speed:2000
	});
	$(window).load(function() {
		$('.list-news').masonry({
		  itemSelector: '.item-brand',
		  columnWidth: 200
		});
		var elem = document.querySelector('.list-news');
		var msnry = new Masonry( elem, {
		  itemSelector: '.new-item',
		  columnWidth: 200
		});
		var msnry = new Masonry( '.list-news', {
		});
	});
	
	$('.bxslider-new-other').bxSlider({
		slideWidth: 240,
		minSlides: 1,
		maxSlides: 4,
		slideMargin: 30,
		pager: false,
		auto:true,
		speed:3000
	});
	 $('.bxslider-partners').bxSlider({
		slideWidth: 143,
		minSlides: 1,
		maxSlides: 6,
		slideMargin: 20,
		pager: false,
		moveSlides:1
	  });
	  menu_mobile();
});
function menu_mobile() {
	$('.toogle-menu').click(function() {
		$('#header').toggleClass('active');
		$('.transform-menu').toggleClass('active');
		$('.toogle-menu').toggleClass('active');
		return false;
	});
	$('.transform-menu').click(function() {
		$('#header').removeClass('active');
		$(this).removeClass('active');
		$('.toogle-menu').removeClass('active');
	});
};