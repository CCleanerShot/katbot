const logoElement = document.getElementById("logo");
logoElement.addEventListener("mouseover", () => logoElement.classList.add("animate-spin-logo"), {once: false})
logoElement.addEventListener("animationiteration", () => logoElement.classList.remove("animate-spin-logo"), {once: false})