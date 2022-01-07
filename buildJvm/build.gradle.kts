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

task("copyScripts", Copy::class) {
    from(filesType("bin"))
    into("/tmp/bin7/bin")
    duplicatesStrategy = DuplicatesStrategy.WARN
    mustRunAfter(":installDist")
}

task("copyLibs", Copy::class) {
    from(filesType("lib"))
    into("/tmp/bin7/lib")
    duplicatesStrategy = DuplicatesStrategy.WARN
    mustRunAfter(":installDist")
}

task ("copyAll") {
    dependsOn(
        ":assemble",
        ":installDist",
        ":copyScripts",
        ":copyLibs"
    )
}

//
//java {
//    toolchain {
//        languageVersion = JavaLanguageVersion.of(17)
//    }
//}
//
//dependencies {
//    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.6.0'
//}
//
//task buildAll {
//    doLast {
//        def bins = fileTree(dir: "$buildDir/..").getFiles()
//        println bins
//    }
//}
//tasks.register('fullBuild', Copy) {
////    from fileTree(dir: "$buildDir/../").getFiles()
////    into file("/tmp/bin6")
////    duplicatesStrategy = DuplicatesStrategy.WARN
//    doLast {
//        println (fileTree(dir: "$buildDir/../").getFiles())
//    }
////    def i=0
////    eachFile {
////        details ->
////            details.setPath( "bbb${i}")
////            i++
////            duplicatesStrategy = DuplicatesStrategy.WARN
////    }
//}

fun Build_gradle.filesType(type: String) = fileTree("$buildDir/..").files.filter {
    it.path.contains("build/install/build_.*/$type".toRegex())
            && it.isFile
}