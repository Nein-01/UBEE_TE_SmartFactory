let xPos = 0, intervalTime = 100;

gsap.timeline()
    .set('.ring-content', { rotationY: 180, cursor: 'grab' }) //set initial rotationY so the parallax jump happens off screen
    .set('.ring-content-item', { // apply transform rotations to each image
        rotateY: (i) => i * -36,
        transformOrigin: '50% 50% 500px',
        z: -500,
        /*backgroundImage: (i) => 'url(https://picsum.photos/id/' + (i + 32) + '/600/400/)',*/
        backgroundPosition: (i) => getBgPos(i),
        backfaceVisibility: 'hidden'
    })
    .from('.ring-content-item', {
        duration: 1.5,
        y: 200,
        opacity: 0,
        stagger: 0.1,
        ease: 'expo'
    })
    .add(() => {
        $('.ring-content-item').on('mouseenter', (e) => {
            let current = e.currentTarget;
            gsap.to('.ring-content-item', { opacity: (i, t) => (t == current) ? 1 : 0.5, ease: 'power3' })
        })
        $('.ring-content-item').on('mouseleave', (e) => {
            gsap.to('.ring-content-item', { opacity: 1, ease: 'power2.inOut' })
        })
    }, '-=0.5')

$(window).on('mousedown touchstart', dragStart);
$(window).on('mouseup touchend', dragEnd);


function dragStart(e) {
    if (e.touches) e.clientX = e.touches[0].clientX;
    xPos = Math.round(e.clientX);
    gsap.set('.ring-content', { cursor: 'grabbing' })
    $(window).on('mousemove touchmove', drag);
}

function dragEnd(e) {
    $(window).off('mousemove touchmove', drag);
    gsap.set('.ring-content', { cursor: 'grab' });
}

function drag(e) {
    if (e.touches) e.clientX = e.touches[0].clientX;

    gsap.to('.ring-content', {
        rotationY: '-=' + ((Math.round(e.clientX) - xPos) % 360),
        onUpdate: () => { gsap.set('.ring-content-item', { backgroundPosition: (i) => getBgPos(i) }) }
    });

    xPos = Math.round(e.clientX);
}


function getBgPos(i) { //returns the background-position string-content to create parallax movement in each image
    return (100 - gsap.utils.wrap(0, 360, gsap.getProperty('.ring-content', 'rotationY') - 180 - i * 36) / 360 * 500) + 'px 0px';
}