export function get(key) {
    return window.sessionStorage.getItem(key);
}

export function set(key, value) {
    window.sessionStorage.setItem(key, value);
}

export function clear() {
    window.sessionStorage.clear();
}

export function remove(key) {
    window.sessionStorage.removeItem(key);
}

export function exists(key) {
    if (window.sessionStorage.getItem(key) == null) {
        return false;
    } else {
        return true;
    }
}