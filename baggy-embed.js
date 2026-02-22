function initializeEmbedMenu(theme) {
  // Click to toggle click-card sections
  document.querySelectorAll(".click-card").forEach(card => {
    const title = card.querySelector(".click-title");
    title.addEventListener("click", () => {
      card.classList.toggle("open");
    });
  });

  // Click to toggle info cards
  document.querySelectorAll(".info-card").forEach(card => {
    const title = card.querySelector(".info-title");
    title.addEventListener("click", () => {
      card.classList.toggle("open");
    });
  });

  // Load JSON when a sub-toggle is clicked
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
              const previewWrap = document.createElement("div");
              previewWrap.className = "preview-wrap";
              const img = document.createElement("img");
              img.src = `https://ms-sar.github.io/${item.preview}`;
              img.className = "preview-img";
              img.alt = item.name;
              const caption = document.createElement("span");
              caption.className = "preview-caption";
              caption.textContent = item.name;
              previewWrap.appendChild(img);
              previewWrap.appendChild(caption);
              trackHeader.appendChild(previewWrap);
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
                  const previewWrap = document.createElement("div");
                  previewWrap.className = "preview-wrap";
                  const varImg = document.createElement("img");
                  varImg.src = `https://ms-sar.github.io/${variation.preview}`;
                  varImg.className = "preview-img";
                  varImg.alt = variation.name;
                  const caption = document.createElement("span");
                  caption.className = "preview-caption";
                  caption.textContent = `${item.name} - ${variation.name}`;
                  previewWrap.appendChild(varImg);
                  previewWrap.appendChild(caption);
                  varLi.appendChild(previewWrap);
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
              const previewWrap = document.createElement("div");
              previewWrap.className = "preview-wrap";
              const img = document.createElement("img");
              img.src = `https://ms-sar.github.io/${item.preview}`;
              img.className = "preview-img";
              img.alt = item.name;
              const caption = document.createElement("span");
              caption.className = "preview-caption";
              caption.textContent = item.name;
              previewWrap.appendChild(img);
              previewWrap.appendChild(caption);
              li.appendChild(previewWrap);
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
