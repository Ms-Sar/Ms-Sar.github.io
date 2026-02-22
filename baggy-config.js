// Baggy Configuration - Shared across main site and embeds
const baggyConfig = {
  // Main menu sections
  sections: [
    {
      id: 'links',
      title: 'Useful Links',
      icon: 'UsefulLinks.png',
      iconAlt: 'UsefulLinks-alt.png',
      type: 'click-card',
      items: [
        {
          text: 'Wreckfest Modding Wiki',
          url: 'https://tads.me.uk/wfwiki/index.php?title=Main_Page',
          icon: 'wiki.png',
          iconAlt: 'wiki-alt.png'
        },
        {
          text: "Mazay's Site",
          url: 'https://mazay.fi/wf/',
          icon: 'mazay.png',
          iconAlt: 'mazay-alt.png'
        },
        {
          text: 'Wreckfest Chronicle',
          url: 'https://docs.google.com/spreadsheets/d/18-g1-I68g1B-9bumvV1HxUVcuD41DBczCyUGH3rdNiQ',
          icon: 'WFchronicle.png',
          iconAlt: 'WFchronicle-alt.png'
        }
      ]
    },
    {
      id: 'tools',
      title: 'Tools',
      icon: 'Tools.png',
      iconAlt: 'Tools-alt.png',
      type: 'click-card',
      items: [
        {
          text: 'Wreckfest Toolbox',
          url: 'https://github.com/gmazy/wreckfest_toolbox',
          icon: 'toolbox.png',
          iconAlt: 'toolbox-alt.png'
        },
        {
          text: 'BagDecompress',
          url: 'https://github.com/gmazy/bag-decompress',
          icon: 'bagdecompress.png',
          iconAlt: 'bagdecompress-alt.png'
        },
        {
          text: 'Breckfest',
          url: 'https://github.com/MaxxWyndham/Breckfest',
          icon: 'breckfest.png',
          iconAlt: 'breckfest-alt.png'
        },
        {
          text: 'WF2 Blender Telemetry',
          url: 'https://youtu.be/dQw4w9WgXcQ',
          icon: 'strtelem.png',
          iconAlt: 'strtelem-alt.png'
        }
      ]
    },
    {
      id: 'steamdb',
      title: 'SteamDB Pages',
      icon: 'steamdb.png',
      iconAlt: 'steamdb-alt.png',
      type: 'click-card',
      items: [
        {
          text: 'WF1 Patch Notes',
          url: 'https://steamdb.info/app/228380/patchnotes/',
          icon: 'PatchNotes.png',
          iconAlt: 'PatchNotes-alt.png'
        },
        {
          text: 'WF1 Manifests',
          url: 'https://steamdb.info/depot/228381/manifests/',
          icon: 'Manifests.png',
          iconAlt: 'Manifests-alt.png'
        },
        {
          text: 'WF1 Server Manifests',
          url: 'https://steamdb.info/depot/361581/manifests/',
          icon: 'ServerManifests.png',
          iconAlt: 'ServerManifests-alt.png'
        },
        {
          text: 'WF2 Patch Notes',
          url: 'https://steamdb.info/app/1203190/patchnotes/',
          icon: 'PatchNotes.png',
          iconAlt: 'PatchNotes-alt.png'
        },
        {
          text: 'WF2 Manifests',
          url: 'https://steamdb.info/depot/1203191/manifests/',
          icon: 'Manifests.png',
          iconAlt: 'Manifests-alt.png'
        },
        {
          text: 'WF2 Server Manifests',
          url: 'https://steamdb.info/depot/3519391/manifests/',
          icon: 'ServerManifests.png',
          iconAlt: 'ServerManifests-alt.png'
        }
      ]
    },
    {
      id: 'wf1',
      title: 'WF1 Info',
      icon: 'wf1.png',
      iconAlt: 'wf1-alt.png',
      type: 'info-card',
      lists: [
        {
          text: 'Vehicle List',
          icon: 'VehicleList.png',
          iconAlt: 'VehicleList-alt.png',
          json: 'wf1_vehicles.json'
        },
        {
          text: 'Track List',
          icon: 'TrackList.png',
          iconAlt: 'TrackList-alt.png',
          json: 'wf1_tracks.json'
        }
      ]
    },
    {
      id: 'wf2',
      title: 'WF2 Info',
      icon: 'wf2.png',
      iconAlt: 'wf2-alt.png',
      type: 'info-card',
      lists: [
        {
          text: 'Vehicle List',
          icon: 'VehicleList.png',
          iconAlt: 'VehicleList-alt.png',
          json: 'wf2_vehicles.json'
        },
        {
          text: 'Track List',
          icon: 'TrackList.png',
          iconAlt: 'TrackList-alt.png',
          json: 'wf2_tracks.json'
        }
      ]
    }
  ]
};

// Function to build Baggy menu from config
function buildBaggyMenu(isEmbed = false, theme = 'green') {
  const assetPath = isEmbed ? 'https://ms-sar.github.io/assets/' : 'assets/';
  const useAltIcons = theme === 'orange';
  
  let html = '';
  
  baggyConfig.sections.forEach(section => {
    const iconFile = useAltIcons ? section.iconAlt : section.icon;
    
    if (section.type === 'click-card') {
      html += `
      <section class="card click-card" data-section="${section.id}">
        <h2 class="card-title click-title">
          <img class="icon-img" src="${assetPath}${iconFile}"> ${section.title}
        </h2>
        <div class="click-panel">`;
      
      section.items.forEach(item => {
        const itemIcon = useAltIcons ? item.iconAlt : item.icon;
        html += `
          <a class="sublink" href="${item.url}" target="_blank">
            <img class="icon-img" src="${assetPath}${itemIcon}"> ${item.text}
          </a>`;
      });
      
      html += `
        </div>
      </section>`;
    }
    
    if (section.type === 'info-card') {
      html += `
      <section class="card info-card" data-info="${section.id}">
        <h2 class="card-title info-title">
          <img class="icon-img" src="${assetPath}${iconFile}"> ${section.title}
        </h2>
        <div class="info-panel">`;
      
      section.lists.forEach(list => {
        const listIcon = useAltIcons ? list.iconAlt : list.icon;
        html += `
          <button class="info-subtoggle" data-json="${list.json}">
            <img class="icon-img" src="${assetPath}${listIcon}"> ${list.text} ▾
          </button>
          <ul class="dynamic-list"></ul>`;
      });
      
      html += `
        </div>
      </section>`;
    }
  });
  
  return html;
}
