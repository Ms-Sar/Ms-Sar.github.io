function initializeEmbedMenu(theme = 'green') {

  function altVariationSrc(path) {
    const full = `https://ms-sar.github.io/${path}`;
    return theme === 'orange' ? full.replace('.webp', '-alt.webp') : full;
  }

  document.querySelectorAll(".click-card").forEach(card => {
    const title = card.querySelector(".click-title");
    title.addEventListener("click", () => {
      card.classList.toggle("open");
    });
  });

  document.querySelectorAll(".info-card").forEach(card => {
    const title = card.querySelector(".info-title");
    title.addEventListener("click", () => {
      card.classList.toggle("open");
    });
  });

  document.querySelectorAll(".info-subtoggle").forEach(btn => {
    btn.addEventListener("click", async () => {
      const jsonFile = btn.dataset.json;
      const list = btn.nextElementSibling;

      const icon = btn.querySelector('.icon-img');
      const iconClone = icon ? icon.cloneNode(true) : null;

      let textWithoutIcon = '';
      btn.childNodes.forEach(node => {
        if (node.nodeType === Node.TEXT_NODE) {
          textWithoutIcon += node.textContent;
        }
      });
      const baseText = textWithoutIcon.replace(/[▾▴⏳]/g, '').trim();

      if (list.dataset.loaded === "true") {
        const isVisible = list.style.display === "block";
        list.style.display = isVisible ? "none" : "block";
        btn.innerHTML = '';
        if (iconClone) btn.appendChild(iconClone);
        btn.appendChild(document.createTextNode(' ' + baseText + (isVisible ? ' ▾' : ' ▴')));
        return;
      }

      btn.innerHTML = '';
      if (iconClone) btn.appendChild(iconClone);
      btn.appendChild(document.createTextNode(' ' + baseText + ' '));
      const loader = document.createElement('span');
      loader.className = 'loader';
      btn.appendChild(loader);

      try {
        const res = await fetch(`https://ms-sar.github.io/${jsonFile}`);
        const data = await res.json();
        list.innerHTML = "";

        const hasTracks = data.some(item => item.variations);

        data.forEach(item => {
          const li = document.createElement("li");

          if (hasTracks) {
            const trackHeader = document.createElement("div");
            trackHeader.className = "track-header";
            trackHeader.appendChild(document.createTextNode(`${item.name} - ${item.id}`));

            if (item.preview) {
              const img = document.createElement("img");
              img.src = `https://ms-sar.github.io/${item.preview}`;
              img.className = "preview-img";
              img.alt = item.name;
              const caption = document.createElement("span");
              caption.className = "preview-caption";
              caption.textContent = item.name;
              trackHeader.appendChild(img);
              trackHeader.appendChild(caption);
            }

            li.appendChild(trackHeader);

            if (item.variations && item.variations.length > 0) {
              const variationsList = document.createElement("ul");
              variationsList.className = "variations-list";

              item.variations.forEach(variation => {
                const varLi = document.createElement("li");
                varLi.className = "variation-item";
                varLi.appendChild(document.createTextNode(`${variation.name} - ${variation.id}`));

                if (variation.preview) {
                  const varImg = document.createElement("img");
                  varImg.src = altVariationSrc(variation.preview);
                  varImg.className = "preview-img";
                  varImg.alt = variation.name;
                  const caption = document.createElement("span");
                  caption.className = "preview-caption";
                  caption.textContent = variation.name;
                  varLi.appendChild(varImg);
                  varLi.appendChild(caption);
                }

                variationsList.appendChild(varLi);
              });

              li.appendChild(variationsList);
            }

            trackHeader.addEventListener("click", () => {
              li.classList.toggle("expanded");
            });

          } else {
            li.appendChild(document.createTextNode(`${item.name} - ${item.id}`));

            if (item.preview) {
              const img = document.createElement("img");
              img.src = `https://ms-sar.github.io/${item.preview}`;
              img.className = "preview-img";
              img.alt = item.name;
              const caption = document.createElement("span");
              caption.className = "preview-caption";
              caption.textContent = item.name;
              li.appendChild(img);
              li.appendChild(caption);
            }
          }

          list.appendChild(li);
        });

        list.dataset.loaded = "true";
        list.style.display = "block";
        btn.innerHTML = '';
        if (iconClone) btn.appendChild(iconClone);
        btn.appendChild(document.createTextNode(' ' + baseText + ' ▴'));

      } catch (error) {
        console.error("Failed to load JSON:", error);
        btn.innerHTML = '';
        if (iconClone) btn.appendChild(iconClone);
        btn.appendChild(document.createTextNode(' ' + baseText + ' ▾ (Error)'));
      }
    });
  });
}
