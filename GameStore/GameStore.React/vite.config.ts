import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            '/api': {
                target: 'https://fluffy-memory-5rg95967pjg2v9p6-5082.app.github.dev/',
                rewrite: (path) => path.replace(/^\/api/, ''),
                changeOrigin: true,
                secure: false
            }
        }
    },
})
