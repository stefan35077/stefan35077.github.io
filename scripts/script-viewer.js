document.addEventListener("DOMContentLoaded", function () {
    const params = new URLSearchParams(window.location.search);
    const project = params.get("project");
    const file = params.get("file");

    const titleEn = document.getElementById("viewerTitleEn");
    const titleNl = document.getElementById("viewerTitleNl");
    const fileName = document.getElementById("viewerFileName");
    const filePathLabel = document.getElementById("viewerPath");
    const statusEn = document.getElementById("viewerStatusEn");
    const statusNl = document.getElementById("viewerStatusNl");
    const codeBlock = document.getElementById("viewerCode");
    const copyButton = document.getElementById("viewerCopy");
    const downloadLink = document.getElementById("viewerDownload");

    const setStatus = (enText, nlText) => {
        if (statusEn) {
            statusEn.textContent = enText;
        }
        if (statusNl) {
            statusNl.textContent = nlText;
        }
    };

    const setTitle = (enText, nlText) => {
        if (titleEn) {
            titleEn.textContent = enText;
        }
        if (titleNl) {
            titleNl.textContent = nlText;
        }
    };

    const labelEn = params.get("labelEn") || file || "Script Viewer";
    const labelNl = params.get("labelNl") || labelEn;

    const projectMap = {
        main: "../scripts",
        hotelhustle: "../scripts/hotelhustle",
        afterparty: "../scripts/afterparty",
        spacescavangers: "../scripts/spacescavangers"
    };

    const baseDir = projectMap[project];
    const isValidFile = file && /^[a-zA-Z0-9._-]+$/.test(file);

    setTitle(labelEn, labelNl);
    setStatus("Loading script...", "Script laden...");

    if (!baseDir || !isValidFile) {
        setStatus("Invalid script selection.", "Ongeldige script selectie.");
        return;
    }

    const resolvedPath = `${baseDir}/${file}`;

    if (fileName) {
        fileName.textContent = file;
    }

    if (filePathLabel) {
        filePathLabel.textContent = resolvedPath.replace("../", "");
    }

    if (downloadLink) {
        downloadLink.href = resolvedPath;
        downloadLink.setAttribute("download", file);
    }

    fetch(resolvedPath, { cache: "no-store" })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.text();
        })
        .then((text) => {
            if (codeBlock) {
                codeBlock.textContent = text;
            }
            setStatus("", "");
        })
        .catch(() => {
            setStatus("Could not load the script.", "Script kon niet worden geladen.");
        });

    if (copyButton) {
        copyButton.addEventListener("click", async () => {
            const textToCopy = codeBlock ? codeBlock.textContent : "";
            if (!textToCopy) {
                setStatus("Nothing to copy yet.", "Nog niets om te kopieren.");
                return;
            }

            try {
                if (navigator.clipboard && navigator.clipboard.writeText) {
                    await navigator.clipboard.writeText(textToCopy);
                } else {
                    throw new Error("Clipboard API unavailable");
                }
                setStatus("Copied to clipboard.", "Gekopieerd naar klembord.");
            } catch (error) {
                setStatus("Copy failed. Use Ctrl+C.", "Kopieren mislukt. Gebruik Ctrl+C.");
            }
        });
    }
});
