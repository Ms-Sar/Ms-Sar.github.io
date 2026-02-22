document.addEventListener('click', e => {
  const clickTitle  = e.target.closest('.click-title');
  const infoTitle   = e.target.closest('.info-title');
  const subToggle   = e.target.closest('.info-subtoggle');
  const trackHeader = e.target.closest('.track-header');

  if (clickTitle)       clickTitle.closest('.click-card').classList.toggle('open');
  else if (infoTitle)   infoTitle.closest('.info-card').classList.toggle('open');
  else if (subToggle)   handleSubToggle(subToggle);
  else if (trackHeader) trackHeader.closest('li').classList.toggle('expanded');
});

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

  btn.innerHTML = `${iconHTML} ${baseText} ⏳`;

  try {
    const res = await fetch(`https://ms-sar.github.io/${jsonFile}`);
    const data = await res.json();
    list.innerHTML = '';
    const hasTracks = data.some(item => item.variations);
    const fragment = document.createDocumentFragment();

    data.forEach(item => {
      const li = document.createElement('li');
      if (hasTracks) {
        const header = document.createElement('div');
        header.className = 'track-header';
        header.appendChild(document.createTextNode(`${item.name} - ${item.id}`));
        if (item.preview) header.appendChild(createImg(item));
        li.appendChild(header);
        if (item.variations?.length) {
          const ul = document.createElement('ul');
          ul.className = 'variations-list';
          item.variations.forEach(v => {
            const varLi = document.createElement('li');
            varLi.className = 'variation-item';
            varLi.appendChild(document.createTextNode(`${v.name} - ${v.id}`));
            if (v.preview) varLi.appendChild(createImg(v));
            ul.appendChild(varLi);
          });
          li.appendChild(ul);
        }
      } else {
        li.appendChild(document.createTextNode(`${item.name} - ${item.id}`));
        if (item.preview) li.appendChild(createImg(item));
      }
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

function createImg(item) {
  const img = document.createElement('img');
  img.src = item.preview;
  img.className = 'preview-img';
  img.alt = item.name;
  img.loading = 'lazy';
  return img;
}
