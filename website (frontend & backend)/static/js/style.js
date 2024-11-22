document.addEventListener('DOMContentLoaded', () => {
    // Р‘СѓСЂРіРµСЂ-РњРµРЅСЋ.
  
    const hamburger = document.getElementById('hamburger');
    hamburger.addEventListener('click', () => {
      hamburger.classList.toggle('is-active');
    });
  
    const handleResize = () => {
      if (window.innerWidth >= 1000) {
        hamburger.classList.remove('is-active');
      }
    };
    window.addEventListener('resize', handleResize);
  
    // AOS Р±РёР±Р»РёРѕС‚РµРєР° РґР»СЏ Р°РЅРёРјР°С†РёРё СЃРєСЂРѕР»Р»Р° СЃС‚СЂР°РЅРёС†С‹.
    AOS.init({
      duration: 1000,
    });
  
    // РЎРєСЂРѕР»Р» РїРѕ РѕРїСЂРµРґРµР»РµРЅРЅС‹Рј СЃРµРєС†РёСЏРј СЃС‚СЂР°РЅРёС†С‹
    let highlightedLink = null;
  
  
  
  
  
    window.scrollAndHighlight = (element, linkId) => {
      if (highlightedLink) {
        highlightedLink.classList.remove('active');
      }
  
      setTimeout(() => {
        element.scrollIntoView({ block: 'start' });
      }, 100);
  
  
      const newHighlightedLink = document.querySelector(`a[href='#${linkId}']`);
      if (newHighlightedLink) {
        newHighlightedLink.classList.add('active');
        highlightedLink = newHighlightedLink;
      }
    };
  
    document.querySelectorAll('a').forEach(link => {
      link.addEventListener('click', () => {
        document.querySelectorAll('a').forEach(link => link.classList.remove('active'));
        link.classList.add('active');
      });
    });
  });