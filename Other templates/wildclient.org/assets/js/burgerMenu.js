const burgerBtn = document.querySelector('.header__burger')
const burgerMenu = document.querySelector('.burger__menu')
const headerOffset = document.querySelector('.header')

burgerBtn.addEventListener(('click'), () => {
    burgerBtn.classList.toggle('header__burger-clicked')
    burgerMenu.classList.toggle('burger__menu--active')
    headerOffset.classList.toggle('header__offset')


    setTimeout(() => {
        burgerBtn.classList.toggle('header__burger-rotate')

    }, 200)
})