document.addEventListener('DOMContentLoaded', () => {
  // nav menu
  const nav = document.querySelector(".nav_sidebar");
  
  document.querySelector("#btnNav").addEventListener("click", () => {
    nav.classList.add("nav_open");
  })

  document.querySelector(".nav_overlay").addEventListener("click", () => {
    nav.classList.remove("nav_open");
  })

  document.querySelector("#btnNavOut").addEventListener("click", () => {
    nav.classList.remove("nav_open");
  })

});
