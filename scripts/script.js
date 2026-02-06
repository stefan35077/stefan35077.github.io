document.addEventListener("DOMContentLoaded", function () {
    const dropMenu = document.getElementById("dropMenu");
    const dropContent = document.getElementById("dropContent");

    if (dropMenu && dropContent) {
        dropMenu.addEventListener("click", function () {
            dropContent.classList.toggle("show");
        });

        window.addEventListener("click", function (event) {
            const clickedInsideMenu = dropContent.contains(event.target);
            const clickedToggle = event.target === dropMenu;

            if (dropContent.classList.contains("show") && !clickedInsideMenu && !clickedToggle) {
                dropContent.classList.remove("show");
            }
        });
    }

    const mediaImages = document.querySelectorAll("img.images");
    const extractYouTubeId = (value) => {
        if (!value) {
            return "";
        }

        const trimmed = value.trim();
        if (/^[a-zA-Z0-9_-]{11}$/.test(trimmed)) {
            return trimmed;
        }

        try {
            const url = new URL(trimmed, window.location.origin);
            const host = url.hostname.replace(/^www\./, "");

            if (host === "youtu.be") {
                return url.pathname.replace("/", "");
            }

            if (host.endsWith("youtube.com")) {
                if (url.searchParams.has("v")) {
                    return url.searchParams.get("v");
                }

                const pathMatch = url.pathname.match(/\/(embed|shorts)\/([^/?]+)/);
                if (pathMatch) {
                    return pathMatch[2];
                }
            }
        } catch (error) {
            return "";
        }

        const fallbackMatch = trimmed.match(/(?:v=|\/embed\/|\/shorts\/|youtu\.be\/)([a-zA-Z0-9_-]{11})/);
        return fallbackMatch ? fallbackMatch[1] : "";
    };

    mediaImages.forEach((img) => {
        const youtubeValue = img.getAttribute("data-youtube");
        const customVideo = img.getAttribute("data-video");

        if (youtubeValue) {
            const videoId = extractYouTubeId(youtubeValue);
            if (!videoId) {
                return;
            }

            const wrapper = document.createElement("div");
            wrapper.className = "video-embed";

            const iframe = document.createElement("iframe");
            iframe.src = `https://www.youtube.com/embed/${videoId}`;
            iframe.title = "YouTube video player";
            iframe.setAttribute(
                "allow",
                "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            );
            iframe.setAttribute("allowfullscreen", "");

            wrapper.appendChild(iframe);
            img.replaceWith(wrapper);
            return;
        }

        if (!customVideo) {
            return;
        }

        const src = img.getAttribute("src") || "";
        const video = document.createElement("video");
        video.className = img.className;
        video.setAttribute("controls", "");
        video.setAttribute("preload", "metadata");
        video.setAttribute("playsinline", "");
        if (src) {
            video.setAttribute("poster", src);
        }
        video.src = customVideo;

        img.replaceWith(video);
    });

    const skillCards = document.querySelectorAll(".skill");
    skillCards.forEach((skill) => {
        const toggle = skill.querySelector(".skillToggle");
        const panel = skill.querySelector(".skillPanel");

        if (!toggle || !panel) {
            return;
        }

        toggle.addEventListener("click", () => {
            const isOpen = skill.classList.toggle("is-open");
            toggle.setAttribute("aria-expanded", isOpen ? "true" : "false");
            panel.setAttribute("aria-hidden", isOpen ? "false" : "true");
        });
    });

    const disableShelf = document.body && document.body.classList.contains("no-script-shelf");
    if (disableShelf || document.getElementById("scriptShelf")) {
        return;
    }

    const scriptPrefix = window.location.pathname.includes("/pages/") ? "../scripts/" : "scripts/";
    const viewerPage = window.location.pathname.includes("/pages/") ? "script-viewer.html" : "pages/script-viewer.html";
    const projectFromBody = document.body ? document.body.dataset.scriptProject : "";

    const normalizeScripts = (items, fallbackProject) => {
        if (!Array.isArray(items)) {
            return [];
        }

        return items
            .map((item) => {
                if (typeof item === "string") {
                    return { file: item };
                }
                return item || null;
            })
            .filter(Boolean)
            .map((item) => {
                const fileName = item.file || "";
                if (!fileName) {
                    return null;
                }
                const baseName = fileName.replace(/\.cs$/i, "");
                const labelEn = item.label && item.label.en ? item.label.en : baseName;
                const labelNl = item.label && item.label.nl ? item.label.nl : labelEn;
                return {
                    ...item,
                    file: fileName,
                    project: item.project || fallbackProject || "",
                    label: { en: labelEn, nl: labelNl }
                };
            })
            .filter(Boolean);
    };

    const buildShelf = (items) => {
        const scripts = normalizeScripts(items, projectFromBody);
        if (!scripts.length) {
            return;
        }

        const buttons = document.getElementById("buttons");
        if (buttons && !document.getElementById("scriptsScrollHint")) {
            const hint = document.createElement("p");
            hint.id = "scriptsScrollHint";
            hint.className = "project-scroll-hint";
            hint.innerHTML = `
                <span class="English">Scroll down for project scripts.</span>
                <span class="Dutch">Scroll naar beneden voor project scripts.</span>
            `;
            buttons.insertAdjacentElement("beforebegin", hint);
        }

        const shelf = document.createElement("section");
        shelf.className = "script-shelf";
        shelf.id = "scriptShelf";

        const title = document.createElement("div");
        title.className = "script-shelf-title";
        title.innerHTML = `
            <h2 class="English">Project Scripts</h2>
            <h2 class="Dutch">Project scripts</h2>
            <p class="English">Open any script like you would in a game engine.</p>
            <p class="Dutch">Open een script alsof je in een game engine zit.</p>
        `;

        const list = document.createElement("div");
        list.className = "script-shelf-list";

        scripts.forEach((item) => {
            const card = document.createElement("div");
            card.className = "script-card";

            const meta = document.createElement("div");
            meta.className = "script-meta";

            const name = document.createElement("p");
            name.className = "script-name";
            name.innerHTML = `
                <span class="English">${item.label.en}</span>
                <span class="Dutch">${item.label.nl}</span>
            `;

            const file = document.createElement("p");
            file.className = "script-file";
            file.textContent = item.file;

            meta.appendChild(name);
            meta.appendChild(file);

            const action = document.createElement("a");
            action.className = "btn script-open";
            const params = new URLSearchParams();
            if (item.project) {
                params.set("project", item.project);
            }
            if (item.file) {
                params.set("file", item.file);
            }
            if (item.label && item.label.en) {
                params.set("labelEn", item.label.en);
            }
            if (item.label && item.label.nl) {
                params.set("labelNl", item.label.nl);
            }
            const viewerUrl = params.toString() ? `${viewerPage}?${params.toString()}` : "";
            const rawBase =
                item.project && item.project !== "main" ? `${scriptPrefix}${item.project}/` : scriptPrefix;
            const rawFallback = item.path ? item.path : `${rawBase}${item.file}`;
            action.href = viewerUrl || rawFallback;
            action.target = "_blank";
            action.rel = "noopener noreferrer";
            action.innerHTML = `
                <span class="English">Open Script</span>
                <span class="Dutch">Open Script</span>
            `;

            card.appendChild(meta);
            card.appendChild(action);
            list.appendChild(card);
        });

        shelf.appendChild(title);
        shelf.appendChild(list);
        document.body.appendChild(shelf);
    };

    if (Array.isArray(window.projectScripts) && window.projectScripts.length) {
        buildShelf(window.projectScripts);
    }
});

