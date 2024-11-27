import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import open from 'open';

// Get base folder for certificates based on OS
const baseFolder = env.APPDATA
    ? path.join(env.APPDATA, 'ASP.NET', 'https')
    : path.join(env.HOME || env.USERPROFILE, '.aspnet', 'https');

const certificateName = "base_pre.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Generate certificate if it doesn't exist
try {
    if (!fs.existsSync(baseFolder)) {
        fs.mkdirSync(baseFolder, { recursive: true });
    }

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        child_process.spawnSync('dotnet', [
            'dev-certs',
            'https',
            '--export-path',
            certFilePath,
            '--format',
            'Pem',
            '--no-password',
        ], { stdio: 'inherit' });
    }
} catch (err) {
    console.warn('Failed to generate HTTPS certificate:', err);
}

// Get target URL for backend API
const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
        ? env.ASPNETCORE_URLS.split(';')[0]
        : 'https://localhost:7140';

// Vite configuration
export default defineConfig({
    plugins: [
        plugin(),
        {
            name: 'open-browser',
            configureServer(server) {
                const protocol = fs.existsSync(keyFilePath) && fs.existsSync(certFilePath) ? 'https' : 'http';
                server.httpServer?.once('listening', () => {
                    const port = server.config.server.port || 5173;
                    const url = `${protocol}://localhost:${port}`;
                    open(url);
                });
            }
        }
    ],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        port: 5173,
        strictPort: true,
        https: fs.existsSync(keyFilePath) && fs.existsSync(certFilePath) ? {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        } : false,
        proxy: {
            '^/weatherforecast': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api': {
                target,
                secure: false,
                changeOrigin: true
            }
        },
        open: true // This is a backup in case the plugin doesn't work
    },
    build: {
        outDir: 'dist',
        sourcemap: true,
        rollupOptions: {
            output: {
                manualChunks: {
                    vendor: ['react', 'react-dom']
                }
            }
        }
    }
});