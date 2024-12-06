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

### 3. Install Docker Desktop
Download and install Docker Desktop from [Docker Desktop](https://www.docker.com/products/docker-desktop). Ensure it is running.

### 4. Set Up HashiCorp Vault
Run the Vault server in Dev mode using Docker:

```bash
$ docker pull hashicorp/vault
$ docker run --cap-add=IPC_LOCK -e 'VAULT_DEV_ROOT_TOKEN_ID=<VAULT_DEV_ROOT_TOKEN>' -p 8200:8200 vault
```

After running this command, note the `Unseal Key` and `Root Token` displayed in the output. 

Set the Vault address:
```bash
$ export VAULT_ADDR='http://127.0.0.1:8200'
```

### Example Output
When the Vault server starts in dev mode, you might see something like this:

```plaintext
Vault server started! Log data will stream in below:
Unseal Key: <YOUR UNSEAL KEY>
Root Token: <YOUR ROOT TOKEN>
Development mode should NOT be used in production installations!
```

### 5. Configure Vault
Log in to Vault using the root token and configure it to store secrets:

```bash
$ vault login <VAULT_DEV_ROOT_TOKEN>
$ vault secrets enable -path=secret kv
$ vault kv put secret/example-key-id value=initial-api-key
```

### 6. Build the Project
Navigate to the main project directory:
```bash
$ cd src/ApiKeyRotation
$ dotnet build
```

### 7. Run the Project
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

### 8. Run Tests
To validate the implementation, navigate to the test directory and run the tests:
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

## Notes for Beginners

1. **Installing .NET SDK**
   - Visit the [.NET SDK download page](https://dotnet.microsoft.com/download) and download the installer for your operating system.
   - After installation, verify it by running:
     ```bash
     dotnet --version
     ```

2. **Installing Docker Desktop**
   - Follow the [Docker Desktop installation guide](https://docs.docker.com/desktop/).
   - Ensure it is running before proceeding with Vault setup.

3. **Running Commands**
   - Open a terminal or command prompt, and ensure you navigate to the correct directory as specified in the instructions.

## Instructions and Guides

1. [HashiCorp Vault Documentation](https://developer.hashicorp.com/vault/docs)
2. [Docker Desktop Setup](https://docs.docker.com/desktop/)
3. [.NET Getting Started](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio)
4. [VaultSharp Library](https://github.com/rajanadar/VaultSharp)

Follow these steps and guides to understand the implementation and how it secures your API key rotation process.

