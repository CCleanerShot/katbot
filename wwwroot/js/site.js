// NOTE: for HTMX anti-forgery (https://khalidabuhakmeh.com/htmx-requests-with-aspnet-core-anti-forgery-tokens)
document.addEventListener("htmx:configRequest", (evt) => {
    let httpVerb = evt.detail.verb.toUpperCase();
    if (httpVerb === 'GET') return;
    
    let antiForgery = htmx.config.antiForgery;

    if (antiForgery) {
        
        // already specified on form, short circuit
        if (evt.detail.parameters[antiForgery.formFieldName])
            return;
        
        if (antiForgery.headerName) {
            evt.detail.headers[antiForgery.headerName]
                = antiForgery.requestToken;
        } else {
             evt.detail.parameters[antiForgery.formFieldName]
                = antiForgery.requestToken;
        }
    }
});

//////////////////////////////////////////////////////////////
// keep animation playing even after you move ur mouse away //
//////////////////////////////////////////////////////////////
const logoElement = document.getElementById("logo");
logoElement.addEventListener("mouseover", () => logoElement.classList.add("animate-spin-logo"), {once: false})
logoElement.addEventListener("animationiteration", () => logoElement.classList.remove("animate-spin-logo"), {once: false})
