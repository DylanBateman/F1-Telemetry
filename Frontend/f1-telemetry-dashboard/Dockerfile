# Build stage
FROM node:20-alpine AS build
WORKDIR /app

COPY Frontend/f1-telemetry-dashboard/package*.json ./
RUN npm ci

COPY Frontend/f1-telemetry-dashboard/ ./
ARG VITE_HUB_URL
ENV VITE_HUB_URL=$VITE_HUB_URL

RUN npm run build

# Runtime stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
