plugins {
    kotlin("jvm") version "1.6.0"
    id("application")
}

version = "unspecified"

repositories {
    mavenCentral()
    google()
}

dependencies {
    implementation(kotlin("stdlib"))
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

subprojects {
    apply(plugin = "application")
    apply(plugin = "kotlin")
    apply(plugin = "java")
    repositories {
        mavenCentral()
    }
    dependencies {
        testImplementation("junit:junit:4.13.2")
        testImplementation("com.github.stefanbirkner:system-rules:1.19.0")
        testImplementation("com.google.truth:truth:1.1.3")
    }
    java {
        toolchain {
            languageVersion.set(JavaLanguageVersion.of(17))
        }
    }

}

fun filesType(type: String) =
    fileTree("$buildDir/..").files.filter {
        it.path.contains("build/install/build_.*/$type".toRegex())
                && it.isFile
    }