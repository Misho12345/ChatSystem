/**
 * Module for managing authentication functionality, including login, registration, token storage, and refresh.
 */
const Auth = (function () {
    let accessToken = null;
    let refreshToken = null;

    /**
     * Stores the access and refresh tokens in memory and local storage.
     * @param {string} access - The access token.
     * @param {string} refresh - The refresh token.
     */
    function storeTokens(access, refresh) {
        accessToken = access;
        refreshToken = refresh;
        localStorage.setItem('accessToken', access);
        localStorage.setItem('refreshToken', refresh);
    }

    /**
     * Loads the access and refresh tokens from local storage into memory.
     */
    function loadTokens() {
        accessToken = localStorage.getItem('accessToken');
        refreshToken = localStorage.getItem('refreshToken');
    }

    /**
     * Clears the access and refresh tokens from memory and local storage.
     */
    function clearTokens() {
        accessToken = null;
        refreshToken = null;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    }

    /**
     * Logs in the user using their identifier and password.
     * Stores the tokens and redirects to the main page upon success.
     * @param {string} identifier - The user's tag or email.
     * @param {string} password - The user's password.
     */
    async function login(identifier, password) {
        try {
            const response = await Api.login({tagOrEmail: identifier, password});
            storeTokens(response.accessToken, response.refreshToken);
            window.location.href = 'main.html';
        } catch (error) {
            console.error('Login failed:', error);
            displayError(error.message || 'Login failed. Please check your credentials.');
        }
    }

    /**
     * Registers a new user with the provided details.
     * Displays a success message and switches to the login form upon success.
     * @param {string} name - The user's name.
     * @param {string} tag - The user's unique tag.
     * @param {string} email - The user's email address.
     * @param {string} password - The user's password.
     */
    async function register(name, tag, email, password) {
        try {
            await Api.register({name, tag, email, password});
            alert('Registration successful! Please login.');
            document.getElementById('registerForm').classList.add('d-none');
            document.getElementById('loginForm').classList.remove('d-none');
            document.getElementById('loginIdentifier').value = email;
        } catch (error) {
            console.error('Registration failed:', error);
            displayError(error.message || 'Registration failed. Please try again.');
        }
    }

    /**
     * Attempts to refresh the access token using the stored refresh token.
     * Redirects to the login page if the refresh fails.
     * @returns {Promise<boolean>} - True if the token was refreshed successfully, false otherwise.
     */
    async function attemptRefresh() {
        if (!refreshToken) return false;
        try {
            const response = await Api.refreshToken({refreshToken});
            storeTokens(response.accessToken, response.refreshToken);
            console.log('Token refreshed successfully.');
            return true;
        } catch (error) {
            console.error('Token refresh failed:', error);
            clearTokens();
            window.location.href = 'index.html';
            return false;
        }
    }

    /**
     * Displays an error message in the UI.
     * @param {string} message - The error message to display.
     */
    function displayError(message) {
        const errorDiv = document.getElementById('errorMessage');
        if (errorDiv) {
            errorDiv.textContent = message;
            errorDiv.classList.remove('d-none');
        }
    }

    /**
     * Hides the error message in the UI.
     */
    function hideError() {
        const errorDiv = document.getElementById('errorMessage');
        if (errorDiv) {
            errorDiv.classList.add('d-none');
        }
    }

    /**
     * Initializes the authentication module.
     * Loads tokens, sets up event listeners for login and registration forms, and redirects unauthenticated users.
     */
    function init() {
        loadTokens();
        if (window.location.pathname.endsWith('main.html') && !Auth.getAccessToken()) {
            window.location.href = 'index.html';
            return;
        }

        const loginForm = document.getElementById('loginForm');
        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                hideError();
                const identifier = document.getElementById('loginIdentifier').value;
                const pass = document.getElementById('loginPassword').value;
                login(identifier, pass);
            });
        }

        const registerForm = document.getElementById('registerForm');
        if (registerForm) {
            registerForm.addEventListener('submit', (e) => {
                e.preventDefault();
                hideError();
                const name = document.getElementById('registerName').value;
                const tag = document.getElementById('registerTag').value;
                const email = document.getElementById('registerEmail').value;
                const pass = document.getElementById('registerPassword').value;
                register(name, tag, email, pass);
            });
        }
    }

    return {
        /**
         * Initializes the authentication module.
         */
        init,

        /**
         * Logs in the user with the provided identifier and password.
         * @param {string} identifier - The user's tag or email.
         * @param {string} password - The user's password.
         */
        login,

        /**
         * Registers a new user with the provided details.
         * @param {string} name - The user's name.
         * @param {string} tag - The user's unique tag.
         * @param {string} email - The user's email address.
         * @param {string} password - The user's password.
         */
        register,

        /**
         * Logs out the user, clears tokens, and redirects to the login page.
         */
        logout: function () {
            if (refreshToken) {
                Api.logout({refreshToken}).catch(err => console.error("Logout API call failed", err));
            }
            clearTokens();
            window.location.href = 'index.html';
        },

        /**
         * Retrieves the stored access token.
         * @returns {string|null} - The access token, or null if not available.
         */
        getAccessToken: () => accessToken,

        /**
         * Retrieves the stored refresh token.
         * @returns {string|null} - The refresh token, or null if not available.
         */
        getRefreshToken: () => refreshToken,

        /**
         * Attempts to refresh the access token using the stored refresh token.
         * @returns {Promise<boolean>} - True if the token was refreshed successfully, false otherwise.
         */
        attemptRefresh,

        /**
         * Checks if the user is logged in.
         * @returns {boolean} - True if the user is logged in, false otherwise.
         */
        isLoggedIn: () => !!accessToken
    };
})();