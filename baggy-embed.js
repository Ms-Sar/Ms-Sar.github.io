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

// Load JSON
document.querySelectorAll(".info-subtoggle").forEach(btn => {
  btn.addEventListener("click", async () => {
    const jsonFile = btn.dataset.json;
    const list = btn.nextElementSibling;

    if (list.dataset.loaded === "true") {
      const isVisible = list.style.display === "block";
      list.style.display = isVisible ? "none" : "block";
      btn.textContent = isVisible 
        ? btn.textContent.replace("▴", "▾")
        : btn.textContent.replace("▾", "▴");
      return;
    }

    btn.textContent = btn.textContent.replace("▾", "⏳");
    
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
          trackHeader.textContent = `${item.name} - ${item.id}`;
          li.appendChild(trackHeader);
          
          if (item.variations && item.variations.length > 0) {
            const variationsList = document.createElement("ul");
            variationsList.className = "variations-list";
            
            item.variations.forEach(variation => {
              const varLi = document.createElement("li");
              varLi.className = "variation-item";
              varLi.textContent = `${variation.name} - ${variation.id}`;
              variationsList.appendChild(varLi);
            });
            
            li.appendChild(variationsList);
          }
          
          trackHeader.addEventListener("click", () => {
            li.classList.toggle("expanded");
          });
        } else {
          li.textContent = `${item.name} - ${item.id}`;
        }

        list.appendChild(li);
      });

      list.dataset.loaded = "true";
      list.style.display = "block";
      btn.textContent = btn.textContent.replace("⏳", "▴");
      
    } catch (error) {
      console.error("Failed to load JSON:", error);
      btn.textContent = btn.textContent.replace("⏳", "▾ (Error)");
    }
  });
});
