plugins {
    kotlin("jvm") version "1.6.0"
    id("application")
}

version = "unspecified"

repositories {
    mavenCentral()
}

dependencies {
    implementation(kotlin("stdlib"))
}

java {
    toolchain {
        languageVersion.set(JavaLanguageVersion.of(17))
    }
}

task("distributeBin", Copy::class) {
    from(filesType("bin"))
    into("$buildDir/distrib/bin")
    duplicatesStrategy = DuplicatesStrategy.WARN
    dependsOn(":build_94_War_kotlin:installDist")
}

task("distributeLib", Copy::class) {
    from(filesType("lib"))
    into("$buildDir/distrib/lib")
    duplicatesStrategy = DuplicatesStrategy.WARN
    dependsOn("installDist")
}

task("copyAll") {
    dependsOn(
        ":distributeBin",
        ":distributeLib"
    )
}

fun filesType(type: String) =
    fileTree("$buildDir/..").files.filter {
        it.path.contains("build/install/build_.*/$type".toRegex())
                && it.isFile
    }