# API Key Rotation Demo

## Overview
This project demonstrates a secure and automated way to rotate API keys for a given application. It uses HashiCorp Vault to manage API keys securely. The primary goal is to showcase how sensitive secrets can be rotated programmatically while adhering to industry best practices for security.

### Key Features
- Secure storage and management of API keys using HashiCorp Vault.
- Automated key generation and rotation.
- Demonstrates a simple yet effective implementation of key rotation logic.
- Includes unit tests to ensure functionality.

## Directory Structure

```plaintext
api-key-rotation/
├── src/
│   ├── ApiKeyRotation/
│   │   ├── ApiKeyRotation.csproj
│   │   ├── Program.cs
│   │   ├── Config/
│   │   │   └── AppSettings.json
│   │   ├── Models/
│   │   │   └── ApiKey.cs
│   │   ├── Services/
│   │   │   ├── VaultService.cs
│   │   │   └── Services.csproj
│   └── tests/
│       └── ApiKeyRotation.Tests/
│           ├── ApiKeyRotation.Tests.csproj
│           ├── Usings.cs
│           └── VaultServiceTests.cs
├── .gitignore
├── api-key-rotation.sln
├── README.md
```

## Prerequisites

1. [.NET SDK 8.0](https://dotnet.microsoft.com/download)
2. [Docker Desktop](https://www.docker.com/products/docker-desktop)
3. HashiCorp Vault Docker image (Dev Mode)
4. Basic understanding of .NET and Vault

## Setting Up the Project

### 1. Clone the Repository
```bash
$ git clone https://github.com/AndrewBergstrom/api-key-rotation.git
$ cd api-key-rotation
```

### 2. Restore Dependencies
Navigate to the root of the solution and run:
```bash
$ dotnet restore
```

### 3. Set Up Docker and Vault
Run the Vault server in Dev mode using Docker:

```bash
$ docker run --cap-add=IPC_LOCK -e 'VAULT_DEV_ROOT_TOKEN_ID=<VAULT_DEV_ROOT_TOKEN>' -p 8200:8200 vault
```

The output will display:
- `Unseal Key`
- `Root Token`

Set the Vault address:
```bash
$ export VAULT_ADDR='http://127.0.0.1:8200'
```

### Example Output
Upon running the Vault server in dev mode, you might see something like this:

```plaintext
Vault server started! Log data will stream in below:
Unseal Key: <YOUR UNSEAL KEY GOES HERE>
Root Token: <YOUR ROOT TOKEN GOES HERE>
Development mode should NOT be used in production installations!
```

### 4. Configure Vault
Run these commands to configure Vault for the project:

```bash
$ vault login <VAULT_DEV_ROOT_TOKEN>
$ vault secrets enable -path=secret kv
$ vault kv put secret/example-key-id value=initial-api-key
```

### 5. Build the Project
Navigate to the main project directory:
```bash
$ cd src/ApiKeyRotation
$ dotnet build
```

### 6. Run the Project
To execute the key rotation logic:
```bash
$ dotnet run
```
Expected output:
```plaintext
Starting API Key Rotation...
info: ApiKeyRotation.Services.VaultService[0]
      Generated new key for Key ID: example-key-id
New API Key: <randomly-generated-key>
```

### 7. Running Tests
Navigate to the test directory:
```bash
$ cd src/tests/ApiKeyRotation.Tests
$ dotnet test
```

## Project Configuration

### `AppSettings.json`
This file contains application configurations such as the Vault address and API key path.
```json
{
  "Vault": {
    "Address": "http://127.0.0.1:8200",
    "KeyPath": "secret/example-key-id"
  }
}
```

### Key Components

#### `Program.cs`
The entry point of the application. It initializes the application configuration and invokes the key rotation logic.

#### `Config`
- **`AppSettings.json`**: Stores the configuration details such as Vault address and the path to the secret.

#### `Models`
- **`ApiKey.cs`**: Defines the structure for an API key object.

#### `Services`
- **`VaultService.cs`**: Handles all interactions with HashiCorp Vault, including reading and writing secrets. It abstracts away the complexities of Vault's API and provides a simple interface for the application to use.
- **`Services.csproj`**: Manages dependencies specific to services and their configurations.

#### `Tests`
- **`VaultServiceTests.cs`**: Contains unit tests for the `VaultService` class to ensure its methods work as expected.
- **`ApiKeyRotation.Tests.csproj`**: Manages dependencies for the test project and links it to the main application.

## Using Docker Desktop for Vault

### Steps to Run Vault in Docker Dev Mode
1. Open Docker Desktop.
2. Pull the Vault image:
   ```bash
   $ docker pull hashicorp/vault
   ```
3. Run the container with Dev mode enabled:
   ```bash
   $ docker run --cap-add=IPC_LOCK -e 'VAULT_DEV_ROOT_TOKEN_ID=<VAULT_DEV_ROOT_TOKEN>' -p 8200:8200 vault
   ```
4. Use the displayed `Unseal Key` and `Root Token` to access Vault.

### Example Output
```plaintext
Vault server started! Log data will stream in below:
Unseal Key: <UNSEAL_KEY>
Root Token: <VAULT_DEV_ROOT_TOKEN>
Development mode should NOT be used in production installations!
```

Use this information to configure your environment:

```bash
$ export VAULT_ADDR='http://127.0.0.1:8200'
```

## Instructions and Guides

1. [HashiCorp Vault Documentation](https://developer.hashicorp.com/vault/docs)
2. [Docker Desktop Setup](https://docs.docker.com/desktop/)
3. [.NET Getting Started](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio)
4. [VaultSharp Library](https://github.com/rajanadar/VaultSharp)

Follow these steps and guides to understand the implementation and how it secures your API key rotation process.

