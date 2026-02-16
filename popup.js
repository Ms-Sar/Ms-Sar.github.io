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

// Typewriter effect - line by line for ASCII art with cursor at end of current line
function typeWriterLines(element, lines, speed, callback) {
  let i = 0;
  element.innerText = '';
  element.style.opacity = '1';
  
  // Create cursor element
  const cursor = document.createElement('span');
  cursor.className = 'typing-cursor';
  cursor.style.display = 'inline-block';
  
  function typeLine() {
    if (i < lines.length) {
      // Remove cursor
      const cursorElements = element.querySelectorAll('.typing-cursor');
      cursorElements.forEach(c => c.remove());
      
      // Add new line
      element.innerText += lines[i] + (i < lines.length - 1 ? '\n' : '');
      
      // Add cursor at the end
      element.appendChild(cursor.cloneNode(true));
      
      i++;
      setTimeout(typeLine, speed);
    } else {
      // Remove cursor when done
      const cursorElements = element.querySelectorAll('.typing-cursor');
      cursorElements.forEach(c => c.remove());
      if (callback) {
        callback();
      }
    }
  }
  typeLine();
}



// Regular typewriter for paragraphs
function typeWriter(element, text, speed, callback) {
  let i = 0;
  element.textContent = '';
  element.style.opacity = '1';
  
  function type() {
    if (i < text.length) {
      element.textContent += text.charAt(i);
      i++;
      setTimeout(type, speed);
    } else if (callback) {
      callback();
    }
  }
  type();
}

document.addEventListener('DOMContentLoaded', async () => {
  const title = document.getElementById('ascii-title');
  const content = document.querySelector('.typewriter-content');
  const paragraph = content.querySelector('p');
  const sections = content.querySelectorAll('section');
  const links = content.querySelectorAll('.main-link');
  
  // Load ASCII art from file
  const response = await fetch('ascii-logo.txt');
  const asciiArt = await response.text();
  const titleLines = asciiArt.split('\n');
  const paragraphText = paragraph.textContent;
  
  // Hide everything initially
  title.style.opacity = '0';
  content.style.opacity = '1';
  paragraph.style.opacity = '0';
  sections.forEach(s => s.style.opacity = '0');
  links.forEach(l => l.style.opacity = '0');
  
  // Start animation sequence
  setTimeout(() => {
    title.style.opacity = '1';
    title.classList.add('typing');
    
    // Type out ASCII art line by line
    typeWriterLines(title, titleLines, 100, () => {
      // Remove cursor after title is done
      setTimeout(() => {
        title.style.borderRight = 'none';
        
        // Start typing paragraph
        paragraph.style.opacity = '1';
        typeWriter(paragraph, paragraphText, 2.5, () => {
          
          // Show first section (My Work)
          setTimeout(() => {
            sections[0].style.opacity = '1';
            sections[0].style.animation = 'fadeInTerminal 0.3s ease forwards';
            
            // Glitch in the links
            setTimeout(() => {
              links[0].style.animation = 'glitchIn 0.6s ease forwards';
            }, 200);
            
            setTimeout(() => {
              links[1].style.animation = 'glitchIn 0.6s ease forwards';
            }, 400);
            
            setTimeout(() => {
              links[2].style.animation = 'glitchIn 0.6s ease forwards';
            }, 600);
            
            // Show second section (About the Sidebar)
            setTimeout(() => {
              sections[1].style.opacity = '1';
              sections[1].style.animation = 'fadeInTerminal 0.3s ease forwards';
            }, 1000);
            
          }, 300);
        });
      }, 500);
    });
  }, 300);
});

