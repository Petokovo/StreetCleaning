document.addEventListener("DOMContentLoaded", function () {
    // Attach behavior to all forms on the page:
    // - Only switch the submit button into "loading" state if the form is valid.
    // - Prevent the common issue where the button stays disabled when validation fails.
    document.querySelectorAll("form").forEach(form => {
        form.addEventListener("submit", function (e) {
            // Find the submit button inside this form (the one we want to lock/show spinner on)
            const btn = form.querySelector("button.po-btn[type=submit]");
            if (!btn) return;

            // Determine validity:
            // Prefer ASP.NET Unobtrusive / jQuery Validate if present,
            // otherwise fall back to native HTML5 validation (checkValidity()).
            let isValid;

            const hasJqValidate =
                typeof window.$ !== "undefined" &&
                typeof window.$.fn?.valid === "function" &&
                window.$(form).data("validator");

            if (hasJqValidate) {
                // jQuery Validate returns true/false and also triggers validation messages
                isValid = window.$(form).valid();
            } else {
                // Native browser validation (required/pattern/minlength/etc.)
                isValid = form.checkValidity();
            }

            // If the form is NOT valid:
            // - Cancel submit to avoid any navigation/postback.
            // - Make sure the button is NOT stuck in loading/disabled state.
            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                resetButton(btn); // Safety net: ensures UI is restored if another handler changed it
                return;
            }

            // Form is valid -> switch button to loading state to prevent double submits
            activateButtonLoading(btn);
        });
    });

    /**
     * Puts a button into "loading" state:
     * - disables it (prevents double click)
     * - shows spinner (Bootstrap)
     * - swaps visible text to data-loading-text, if provided
     */
    function activateButtonLoading(btn) {
        // Mark as loading (useful if you later want to ignore input-driven toggles)
        btn.dataset.loading = "1";

        // Disable to prevent multiple submits/clicks
        btn.disabled = true;

        // Show spinner if present
        const spinner = btn.querySelector(".spinner-border");
        if (spinner) {
            spinner.classList.remove("d-none");
        }

        // Replace button text with loading text if configured
        const text = btn.querySelector(".btn-text");
        if (text && btn.dataset.loadingText) {
            text.textContent = btn.dataset.loadingText;
        }
    }

    /**
     * Restores a button back to its normal state:
     * - enables it
     * - hides spinner
     * - restores default text (currently hard-coded)
     *
     * NOTE: For full reusability, consider storing the original text in
     * btn.dataset.originalText on first run and restoring that instead.
     */
    function resetButton(btn) {
        // Clear loading marker
        delete btn.dataset.loading;

        // Re-enable the button
        btn.disabled = false;

        // Hide spinner if present
        const spinner = btn.querySelector(".spinner-border");
        if (spinner) {
            spinner.classList.add("d-none");
        }

        // Restore button text
        const text = btn.querySelector(".btn-text");
        if (text) {
            text.textContent = "Vyhľadať"; // TODO: optionally restore from data-original-text for multiple buttons/forms
        }
    }
});

//tooltips
document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl)
    })
});