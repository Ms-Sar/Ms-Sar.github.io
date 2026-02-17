// CRT Power-on effect
window.addEventListener('load', () => {
  // Page starts with black background, hide content
  document.body.style.visibility = 'hidden';
  
  // Create power-on overlay
  const powerup = document.createElement('div');
  powerup.className = 'crt-powerup';
  document.body.appendChild(powerup);
  
  // Create boot scanline
  const scanline = document.createElement('div');
  scanline.className = 'crt-scanline-boot';
  document.body.appendChild(scanline);
  
  // Show content and switch to normal background
  setTimeout(() => {
    document.body.style.visibility = 'visible';
    document.body.classList.add('powered-on');
  }, 400);
  
  // Remove CRT elements
  setTimeout(() => {
    powerup.remove();
    scanline.remove();
  }, 1000);
});


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

// ASCII Art for desktop
const asciiArtLines = [
  "   ███████╗████████╗██████╗·███╗···███╗·██████╗·██████╗·███████╗",
  "   ██╔════╝╚══██╔══╝██╔══██╗████╗·████║██╔═══██╗██╔══██╗██╔════╝",
  "   ███████╗···██║···██████╔╝██╔████╔██║██║···██║██║··██║███████╗",
  "   ╚════██║···██║···██╔══██╗██║╚██╔╝██║██║···██║██║··██║╚════██║",
  "   ███████║···██║···██║··██║██║·╚═╝·██║╚██████╔╝██████╔╝███████║",
  "   ╚══════╝···╚═╝···╚═╝··╚═╝╚═╝·····╚═╝·╚═════╝·╚═════╝·╚══════╝"
];

// ASCII Art for mobile - with extra spacing
const asciiArtLinesMobile = [
  "   ███████╗████████╗██████╗·███╗···███╗··██████╗·██████╗·███████╗",
  "   ██╔════╝╚══██╔══╝██╔══██╗████╗·████║██╔═══██╗██╔══██╗██╔════╝",
  "   ███████╗····██║···██████╔╝██╔████╔██║██║···██║██║··██║███████╗",
  "   ╚════██║····██║···██╔══██╗██║╚██╔╝██║██║···██║██║··██║╚════██║",
  "   ███████║····██║···██║··██║██║·╚═╝·██║╚██████╔╝██████╔╝███████║",
  "   ╚══════╝····╚═╝···╚═╝··╚═╝╚═╝·····╚═╝·╚═════╝·╚═════╝·╚══════╝"
];

// Typewriter effect - character by character for ASCII art with cursor (FAST)
function typeWriterASCII(element, lines, charSpeed, lineDelay, callback) {
  let lineIndex = 0;
  let charIndex = 0;
  let currentText = '';
  let lastTime = performance.now();
  let accumulated = 0;
  
  // Create cursor element
  const cursor = document.createElement('span');
  cursor.className = 'typing-cursor';
  
  element.innerText = '';
  element.style.opacity = '1';
  element.appendChild(cursor);
  
  function typeChar(currentTime) {
    const deltaTime = currentTime - lastTime;
    lastTime = currentTime;
    accumulated += deltaTime;
    
    // Type multiple characters per frame if needed
    while (accumulated >= charSpeed && lineIndex < lines.length) {
      const currentLine = lines[lineIndex];
      
      if (charIndex < currentLine.length) {
        // Type next character
        currentText += currentLine[charIndex];
        charIndex++;
        accumulated -= charSpeed;
      } else {
        // Line complete, move to next line
        currentText += '\n';
        lineIndex++;
        charIndex = 0;
        accumulated -= lineDelay;
        break; // Wait for line delay
      }
    }
    
    element.innerText = currentText;
    element.appendChild(cursor);
    
    if (lineIndex < lines.length) {
      requestAnimationFrame(typeChar);
    } else {
      // All lines complete
      if (callback) {
        callback(cursor);
      }
    }
  }
  
  requestAnimationFrame(typeChar);
}

// Typewriter for paragraphs with moving cursor (FAST) - preserves images and HTML
function typeWriter(element, html, speed, cursor, callback) {
  let i = 0;
  let lastTime = performance.now();
  let accumulated = 0;
  
  // Store the image if it exists
  const img = element.querySelector('img');
  
  // Create a temporary div to parse HTML
  const temp = document.createElement('div');
  temp.innerHTML = html;
  const textContent = temp.textContent || temp.innerText;
  
  element.textContent = '';
  element.style.opacity = '1';
  
  // Re-add the image if it existed
  if (img) {
    element.appendChild(img);
  }
  
  element.appendChild(cursor);
  
  function type(currentTime) {
    const deltaTime = currentTime - lastTime;
    lastTime = currentTime;
    accumulated += deltaTime;
    
    // Type multiple characters per frame if needed
    while (accumulated >= speed && i < textContent.length) {
      i++;
      accumulated -= speed;
    }
    
    if (i > 0) {
      const currentText = textContent.substring(0, i);
      
      // Clear and rebuild with image and HTML
      element.innerHTML = '';
      if (img) {
        element.appendChild(img);
      }
      
      // Replace line breaks in the typed portion
      const htmlText = html.substring(0, findHTMLPosition(html, i));
      const span = document.createElement('span');
      span.innerHTML = htmlText;
      element.appendChild(span);
      element.appendChild(cursor);
    }
    
    if (i < textContent.length) {
      requestAnimationFrame(type);
    } else {
      // Remove cursor when done, set final HTML
      cursor.remove();
      element.innerHTML = '';
      if (img) {
        element.appendChild(img);
      }
      const span = document.createElement('span');
      span.innerHTML = html;
      element.appendChild(span);
      
      if (callback) {
        callback();
      }
    }
  }
  
  // Helper to find HTML position based on text position
  function findHTMLPosition(html, textPos) {
    let textCount = 0;
    let htmlPos = 0;
    let inTag = false;
    
    while (htmlPos < html.length && textCount < textPos) {
      if (html[htmlPos] === '<') {
        inTag = true;
      } else if (html[htmlPos] === '>') {
        inTag = false;
        htmlPos++;
        continue;
      }
      
      if (!inTag) {
        textCount++;
      }
      htmlPos++;
    }
    
    return htmlPos;
  }
  
  requestAnimationFrame(type);
}



document.addEventListener('DOMContentLoaded', async () => {
  const title = document.getElementById('ascii-title');
  const content = document.querySelector('.typewriter-content');
  const paragraph = content.querySelector('p');
  const sections = content.querySelectorAll('section');
  const links = content.querySelectorAll('.main-link');
  
// Get HTML content without the image
const paragraphHTML = paragraph.innerHTML.replace(/<img[^>]*>/g, '').trim();

  // Hide everything initially
  title.style.opacity = '0';
  content.style.opacity = '1';
  paragraph.style.opacity = '0';
  sections.forEach(s => s.style.opacity = '0');
  links.forEach(l => l.style.opacity = '0');
  
  // Start animation sequence - delayed to let CRT effect finish
  setTimeout(() => {
    title.style.opacity = '1';
    title.classList.add('typing');
    
    // Type out ASCII art character by character
    typeWriterASCII(title, asciiArtLines, 5, 20, (cursor) => {
      // ASCII art done, move cursor to paragraph
      setTimeout(() => {
        paragraph.style.opacity = '1';
        
        // Move cursor to paragraph and start typing
        typeWriter(paragraph, paragraphHTML, 2.25, cursor, () => {
          
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
      }, 300);
    });
  }, 900); // Start typing after CRT effect (900ms)
});

// ========== MOBILE BAGGY TOGGLE ==========
const mobileBaggyBtn = document.getElementById('mobileBaggyBtn');
const sidebar = document.querySelector('.sticky-sidebar');

// Open sidebar
mobileBaggyBtn.addEventListener('click', () => {
  sidebar.classList.add('mobile-open');
  mobileBaggyBtn.classList.add('hidden');
});

// Close sidebar - click close button (::before pseudo-element)
sidebar.addEventListener('click', (e) => {
  const rect = sidebar.getBoundingClientRect();
  if (e.clientY < rect.top + 50) { // Close button area (top 50px)
    sidebar.classList.remove('mobile-open');
    mobileBaggyBtn.classList.remove('hidden');
  }
});


// Close sidebar - click close button (::before pseudo-element)
sidebar.addEventListener('click', (e) => {
  const rect = sidebar.getBoundingClientRect();
  if (e.clientY < rect.top + 50) { // Close button area (top 50px)
    sidebar.classList.remove('mobile-open');
    overlay.classList.remove('active');
    mobileBaggyBtn.classList.remove('hidden');
  }
});


// ========== BAGGY EMBED MODAL ==========
const embedBtn = document.getElementById('embedBtn');
const embedModal = document.getElementById('embedModal');
const embedClose = document.querySelector('.embed-close');
const embedModalContent = document.querySelector('.embed-modal-content');
const embedCode = document.getElementById('embedCode');
const copyEmbedBtn = document.getElementById('copyEmbedBtn');
const copyFeedback = document.getElementById('copyFeedback');
const closeModalBtn = document.getElementById('closeModalBtn');
const themeButtons = document.querySelectorAll('.theme-btn');

// Move modal to body to escape stacking context
document.body.appendChild(embedModal);

let currentTheme = 'green';


const embedCodes = {
  green: `<iframe src="https://ms-sar.github.io/baggy-embed-green.html" 
  width="400" height="600" 
  style="border: 2px solid #00ff41; border-radius: 8px; box-shadow: 0 0 20px rgba(0,255,65,0.3);"
  title="Baggy - Wreckfest Modding Companion">
</iframe>`,
  orange: `<iframe src="https://ms-sar.github.io/baggy-embed-orange.html" 
  width="400" height="600" 
  style="border: 2px solid #ff8800; border-radius: 8px; box-shadow: 0 0 20px rgba(255,136,0,0.3);"
  title="Baggy - Wreckfest Modding Companion">
</iframe>`
};

// Close modal function
function closeModal() {
  embedModal.style.display = 'none';
}

// Open modal
embedBtn.addEventListener('click', () => {
  embedModal.style.display = 'block';
  embedCode.value = embedCodes[currentTheme];
});

// Close on X button
embedClose.addEventListener('click', closeModal);

// Close on bottom close button
closeModalBtn.addEventListener('click', closeModal);

// Close when clicking outside the modal content
embedModal.addEventListener('click', (e) => {
  if (e.target === embedModal) {
    closeModal();
  }
});

// Prevent clicks inside modal content from closing
embedModalContent.addEventListener('click', (e) => {
  e.stopPropagation();
});

// Close with Escape key
document.addEventListener('keydown', (e) => {
  if (e.key === 'Escape' && embedModal.style.display === 'block') {
    closeModal();
  }
});

// Theme selection
themeButtons.forEach(btn => {
  btn.addEventListener('click', () => {
    themeButtons.forEach(b => b.classList.remove('active'));
    btn.classList.add('active');
    currentTheme = btn.dataset.theme;
    embedCode.value = embedCodes[currentTheme];
  });
});

// Copy to clipboard
copyEmbedBtn.addEventListener('click', () => {
  embedCode.select();
  document.execCommand('copy');
  
  copyFeedback.classList.add('show');
  setTimeout(() => {
    copyFeedback.classList.remove('show');
  }, 2000);
});

// Close when clicking outside the modal content
embedModal.addEventListener('click', (e) => {
  console.log('Modal clicked!', e.target, e.target === embedModal);
  if (e.target === embedModal) {
    closeModal();
  }
});
