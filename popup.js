document.addEventListener('DOMContentLoaded', () => {
  const container = document.getElementById('baggy-menu-container');
  if (container) {
    container.innerHTML = buildBaggyMenu(false, 'green');
    initializeBaggyMenu();
  }
});

function initializeBaggyMenu() {
  const menuContainer = document.getElementById('baggy-menu-container');
  if (!menuContainer) return;

  menuContainer.querySelectorAll('.info-subtoggle').forEach(btn => {
    btn.addEventListener('click', () => handleSubToggle(btn));
  });

  menuContainer.addEventListener('click', e => {
    const clickTitle = e.target.closest('.click-title');
    const infoTitle = e.target.closest('.info-title');
    const trackHeader = e.target.closest('.track-header');

    if (clickTitle) clickTitle.closest('.click-card').classList.toggle('open');
    else if (infoTitle) infoTitle.closest('.info-card').classList.toggle('open');
    else if (trackHeader) trackHeader.closest('li').classList.toggle('expanded');
  });
}

async function handleSubToggle(btn) {
  const jsonFile = btn.dataset.json;
  const list = btn.nextElementSibling;
  const icon = btn.querySelector('.icon-img');
  const iconHTML = icon ? icon.outerHTML : '';
  const baseText = [...btn.childNodes]
    .filter(n => n.nodeType === Node.TEXT_NODE)
    .map(n => n.textContent)
    .join('')
    .replace(/[▾▴⏳]/g, '')
    .trim();

  if (list.dataset.loaded === 'true') {
    const isVisible = list.style.display === 'block';
    list.style.display = isVisible ? 'none' : 'block';
    btn.innerHTML = `${iconHTML} ${baseText} ${isVisible ? '▾' : '▴'}`;
    return;
  }

  btn.innerHTML = `${iconHTML} ${baseText} <span class="loader"></span>`;

  try {
    const res = await fetch(jsonFile);
    const data = await res.json();
    list.innerHTML = '';
    const hasTracks = data.some(item => item.variations);
    const fragment = document.createDocumentFragment();
    data.forEach(item => {
      const li = document.createElement('li');
      li.appendChild(hasTracks ? buildTrackItem(item) : buildVehicleItem(item));
      fragment.appendChild(li);
    });
    list.appendChild(fragment);
    list.dataset.loaded = 'true';
    list.style.display = 'block';
    btn.innerHTML = `${iconHTML} ${baseText} ▴`;
  } catch (err) {
    console.error('Failed to load JSON:', err);
    btn.innerHTML = `${iconHTML} ${baseText} ▾ (Error)`;
  }
}

function buildTrackItem(item) {
  const container = document.createElement('div');
  const header = document.createElement('div');
  header.className = 'track-header';
  header.appendChild(document.createTextNode(`${item.name} - ${item.id}`));
  if (item.preview) header.appendChild(createPreviewImg(item));
  container.appendChild(header);

  if (item.variations?.length) {
    const ul = document.createElement('ul');
    ul.className = 'variations-list';
    item.variations.forEach(v => {
      const varLi = document.createElement('li');
      varLi.className = 'variation-item';
      varLi.appendChild(document.createTextNode(`${v.name} - ${v.id}`));
      if (v.preview) varLi.appendChild(createPreviewImg(v));
      ul.appendChild(varLi);
    });
    container.appendChild(ul);
  }
  return container;
}

function buildVehicleItem(item) {
  const container = document.createElement('div');
  container.appendChild(document.createTextNode(`${item.name} - ${item.id}`));
  if (item.preview) container.appendChild(createPreviewImg(item));
  return container;
}

function createPreviewImg(item) {
  const img = document.createElement('img');
  img.src = item.preview;
  img.className = 'preview-img';
  img.alt = item.name;
  img.loading = 'lazy';
  return img;
}

// CRT Power-on effect
window.addEventListener('load', () => {
  document.body.style.visibility = 'hidden';
  const powerup = document.createElement('div');
  powerup.className = 'crt-powerup';
  const scanline = document.createElement('div');
  scanline.className = 'crt-scanline-boot';
  document.body.append(powerup, scanline);
  setTimeout(() => {
    document.body.style.visibility = 'visible';
    document.body.classList.add('powered-on');
  }, 400);
  setTimeout(() => { powerup.remove(); scanline.remove(); }, 1000);
});

document.getElementById('search')?.addEventListener('keydown', e => {
  if (e.key === 'Enter') e.target.form.submit();
});

const asciiArtLines = [
  " ███████╗████████╗██████╗·███╗···███╗·██████╗·██████╗·███████╗",
  " ██╔════╝╚══██╔══╝██╔══██╗████╗·████║██╔═══██╗██╔══██╗██╔════╝",
  " ███████╗···██║···██████╔╝██╔████╔██║██║···██║██║··██║███████╗",
  " ╚════██║···██║···██╔══██╗██║╚██╔╝██║██║···██║██║··██║╚════██║",
  " ███████║···██║···██║··██║██║·╚═╝·██║╚██████╔╝██████╔╝███████║",
  " ╚══════╝···╚═╝···╚═╝··╚═╝╚═╝·····╚═╝·╚═════╝·╚═════╝·╚══════╝"
];

function typeWriterASCII(element, lines, charSpeed, lineDelay, callback) {
  let lineIndex = 0, charIndex = 0, currentText = '';
  let lastTime = performance.now(), accumulated = 0;
  const cursor = document.createElement('span');
  cursor.className = 'typing-cursor';
  element.innerText = '';
  element.style.opacity = '1';
  element.appendChild(cursor);

  function typeChar(currentTime) {
    const delta = currentTime - lastTime;
    lastTime = currentTime;
    accumulated += delta;
    while (accumulated >= charSpeed && lineIndex < lines.length) {
      const line = lines[lineIndex];
      if (charIndex < line.length) {
        currentText += line[charIndex++];
        accumulated -= charSpeed;
      } else {
        currentText += '\n';
        lineIndex++; charIndex = 0;
        accumulated -= lineDelay;
        break;
      }
    }
    element.innerText = currentText;
    element.appendChild(cursor);
    if (lineIndex < lines.length) requestAnimationFrame(typeChar);
    else if (callback) callback(cursor);
  }
  requestAnimationFrame(typeChar);
}

function findHTMLPosition(html, textPos) {
  let textCount = 0, htmlPos = 0, inTag = false;
  while (htmlPos < html.length && textCount < textPos) {
    if (html[htmlPos] === '<') inTag = true;
    else if (html[htmlPos] === '>') { inTag = false; htmlPos++; continue; }
    if (!inTag) textCount++;
    htmlPos++;
  }
  return htmlPos;
}

function typeWriter(element, html, speed, cursor, callback) {
  let i = 0, lastTime = performance.now(), accumulated = 0;
  const img = element.querySelector('img');
  const temp = document.createElement('div');
  temp.innerHTML = html;
  const textContent = temp.textContent || temp.innerText;
  element.textContent = '';
  element.style.opacity = '1';
  if (img) element.appendChild(img);
  element.appendChild(cursor);

  function type(currentTime) {
    const delta = currentTime - lastTime;
    lastTime = currentTime;
    accumulated += delta;
    while (accumulated >= speed && i < textContent.length) { i++; accumulated -= speed; }

    if (i > 0) {
      const htmlText = html.substring(0, findHTMLPosition(html, i));
      element.innerHTML = '';
      if (img) element.appendChild(img);
      const span = document.createElement('span');
      span.innerHTML = htmlText;
      element.appendChild(span);
      element.appendChild(cursor);
    }

    if (i < textContent.length) {
      requestAnimationFrame(type);
    } else {
      cursor.remove();
      element.innerHTML = '';
      if (img) element.appendChild(img);
      const span = document.createElement('span');
      span.innerHTML = html;
      element.appendChild(span);
      if (callback) callback();
    }
  }
  requestAnimationFrame(type);
}

document.addEventListener('DOMContentLoaded', () => {
  const title = document.getElementById('ascii-title');
  const content = document.querySelector('.typewriter-content');
  const paragraph = content.querySelector('p');
  const sections = content.querySelectorAll('section');
  const links = content.querySelectorAll('.main-link');
  const paragraphHTML = paragraph.innerHTML.replace(/<img[^>]*>/g, '').trim();

  title.style.opacity = '0';
  content.style.opacity = '1';
  paragraph.style.opacity = '0';
  sections.forEach(s => s.style.opacity = '0');
  links.forEach(l => l.style.opacity = '0');

  setTimeout(() => {
    title.style.opacity = '1';
    title.classList.add('typing');
    typeWriterASCII(title, asciiArtLines, 5, 20, cursor => {
      setTimeout(() => {
        paragraph.style.opacity = '1';
        typeWriter(paragraph, paragraphHTML, 2.25, cursor, () => {
          setTimeout(() => {
            sections[0].style.opacity = '1';
            sections[0].style.animation = 'fadeInTerminal 0.3s ease forwards';
            [200, 400, 600].forEach((delay, i) => {
              setTimeout(() => { links[i].style.animation = 'glitchIn 0.6s ease forwards'; }, delay);
            });
            setTimeout(() => {
              sections[1].style.opacity = '1';
              sections[1].style.animation = 'fadeInTerminal 0.3s ease forwards';
            }, 1000);
          }, 300);
        });
      }, 300);
    });
  }, 900);
});

// Mobile sidebar
const mobileBaggyBtn = document.getElementById('mobileBaggyBtn');
const sidebar = document.querySelector('.sticky-sidebar');

mobileBaggyBtn?.addEventListener('click', () => {
  sidebar.classList.add('mobile-open');
  mobileBaggyBtn.classList.add('hidden');
});

sidebar?.addEventListener('click', e => {
  const rect = sidebar.getBoundingClientRect();
  if (e.clientY < rect.top + 50) {
    sidebar.classList.remove('mobile-open');
    mobileBaggyBtn.classList.remove('hidden');
  }
});

// Embed modal
const embedBtn = document.getElementById('embedBtn');
const embedModal = document.getElementById('embedModal');
const embedClose = document.querySelector('.embed-close');
const embedModalContent = document.querySelector('.embed-modal-content');
const embedCode = document.getElementById('embedCode');
const copyEmbedBtn = document.getElementById('copyEmbedBtn');
const copyFeedback = document.getElementById('copyFeedback');
const closeModalBtn = document.getElementById('closeModalBtn');
const themeButtons = document.querySelectorAll('.theme-btn');

document.body.appendChild(embedModal);
let currentTheme = 'green';

const embedCodes = {
  green: `<iframe src="https://ms-sar.github.io/baggy-embed-green.html" 
  width="400" height="600" 
  style="border: 2px solid #00ff41; border-radius: 8px; box-shadow: 0 0 20px rgba(0,255,65,0.3);"
  title="STRmods: Baggy - WF Modding Companion">
</iframe>`,
  orange: `<iframe src="https://ms-sar.github.io/baggy-embed-orange.html" 
  width="400" height="600" 
  style="border: 2px solid #ff8800; border-radius: 8px; box-shadow: 0 0 20px rgba(255,136,0,0.3);"
  title="STRmods: Baggy - WF Modding Companion">
</iframe>`
};

function closeModal() { embedModal.style.display = 'none'; }

embedBtn?.addEventListener('click', () => {
  embedModal.style.display = 'block';
  embedCode.value = embedCodes[currentTheme];
});
embedClose?.addEventListener('click', closeModal);
closeModalBtn?.addEventListener('click', closeModal);
embedModal?.addEventListener('click', e => { if (e.target === embedModal) closeModal(); });
embedModalContent?.addEventListener('click', e => e.stopPropagation());
document.addEventListener('keydown', e => {
  if (e.key === 'Escape' && embedModal.style.display === 'block') closeModal();
});

themeButtons.forEach(btn => {
  btn.addEventListener('click', () => {
    themeButtons.forEach(b => b.classList.remove('active'));
    btn.classList.add('active');
    currentTheme = btn.dataset.theme;
    embedCode.value = embedCodes[currentTheme];
  });
});

copyEmbedBtn?.addEventListener('click', () => {
  navigator.clipboard?.writeText(embedCode.value).catch(() => {
    embedCode.select();
    document.execCommand('copy');
  });
  copyFeedback.classList.add('show');
  setTimeout(() => copyFeedback.classList.remove('show'), 2000);
});
