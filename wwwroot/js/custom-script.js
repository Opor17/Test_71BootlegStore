"use strict";

// ACTIVE NAV

const activePage = window.location.href;
const navLinks = document.querySelectorAll(".side-menu");

for (let i = 0; i < navLinks.length; i++) {
  navLinks[i].href === activePage
    ? navLinks[i].classList.add("side-menu--active")
    : undefined;
  navLinks[i].classList.contains("side-menu--active")
    ? navLinks[i].parentElement.parentElement.classList.add(
        "side-menu__sub-open"
      )
    : null;
}

// SELECT MOVEMENT FORM

/*const movementSelectEl = document.querySelector("#movement-select");
const movementFormEl = document.querySelectorAll(".movement-form");

const btnGeneral = document.querySelector("#btn-general");
const btn321Qi = document.querySelector("#btn-321qi");*/

/*movementSelectEl?.addEventListener("change", () => {
  console.log(movementSelectEl.value);

  for (let i = 0; i < movementFormEl.length; i++) {
    movementFormEl[i].getAttribute("id") == movementSelectEl.value
      ? movementFormEl[i].classList.remove("hidden")
      : movementFormEl[i].classList.add("hidden");
  }

  if (movementSelectEl.value == "321qi") {
    btn321Qi?.classList.remove("hidden");
    btnGeneral.classList.add("hidden");
  } else {
    btn321Qi?.classList.add("hidden");
    btnGeneral.classList.remove("hidden");
  }
});*/
