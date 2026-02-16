// Search form submit
document.getElementById("search").addEventListener("keydown", e => {
  if (e.key === "Enter") {
    e.target.form.submit();
  }
});

// Click to toggle click-card sections (Useful Links, Tools, SteamDB)
document.querySelectorAll(".click-card").forEach(card => {
  const title = card.querySelector(".click-title");
  title.addEventListener("click", () => {
    card.classList.toggle("open");
  });
});

// Click to toggle WF1/WF2 info cards
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

    // If already loaded, just toggle visibility
    if (list.dataset.loaded === "true") {
      const isVisible = list.style.display === "block";
      list.style.display = isVisible ? "none" : "block";
      btn.textContent = isVisible 
        ? btn.textContent.replace("▴", "▾")
        : btn.textContent.replace("▾", "▴");
      return;
    }

    // Show loading state
    btn.textContent = btn.textContent.replace("▾", "⏳");
    
    try {
      const res = await fetch(jsonFile);
      const data = await res.json();

      list.innerHTML = "";

      // Check if this is a tracks JSON (has variations)
      const hasTracks = data.some(item => item.variations);

      data.forEach(item => {
        const li = document.createElement("li");
        
        if (hasTracks) {
          // Track with variations
          const trackHeader = document.createElement("div");
          trackHeader.className = "track-header";
          
          const trackText = document.createTextNode(`${item.name} - ${item.id}`);
          trackHeader.appendChild(trackText);
          
          if (item.preview) {
            const img = document.createElement("img");
            img.src = item.preview;
            img.className = "preview-img";
            img.alt = item.name;
            trackHeader.appendChild(img);
          }
          
          li.appendChild(trackHeader);
          
          // Variations list
          if (item.variations && item.variations.length > 0) {
            const variationsList = document.createElement("ul");
            variationsList.className = "variations-list";
            
            item.variations.forEach(variation => {
              const varLi = document.createElement("li");
              varLi.className = "variation-item";
              
              const varText = document.createTextNode(`${variation.name} - ${variation.id}`);
              varLi.appendChild(varText);
              
              if (variation.preview) {
                const varImg = document.createElement("img");
                varImg.src = variation.preview;
                varImg.className = "preview-img";
                varImg.alt = variation.name;
                varLi.appendChild(varImg);
              }
              
              variationsList.appendChild(varLi);
            });
            
            li.appendChild(variationsList);
          }
          
          // Toggle variations on click
          trackHeader.addEventListener("click", () => {
            li.classList.toggle("expanded");
          });
        } else {
          // Vehicle (no variations)
          const vehicleText = document.createTextNode(`${item.name} - ${item.id}`);
          li.appendChild(vehicleText);
          
          if (item.preview) {
            const img = document.createElement("img");
            img.src = item.preview;
            img.className = "preview-img";
            img.alt = item.name;
            li.appendChild(img);
          }
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

// Typewriter effect for main content
document.addEventListener('DOMContentLoaded', () => {
  const title = document.querySelector('.typewriter-title');
  const content = document.querySelector('.typewriter-content');
  
  // Start hidden
  title.style.opacity = '0';
  content.style.opacity = '0';
  
  // Animate title first
  setTimeout(() => {
    title.style.opacity = '1';
    title.classList.add('typing');
  }, 300);
  
  // Then animate content
  setTimeout(() => {
    content.style.opacity = '1';
    content.classList.add('typing');
  }, 2000);
});
