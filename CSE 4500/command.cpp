// command.cpp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
// CommandPatternExample.cpp
// Demonstrates the Command design pattern with TV and Stereo receivers in C++.

#include <iostream>
#include <stdexcept>

// 1) Command interface: encapsulated action
class Command {
public:
    virtual void execute() = 0;
    virtual ~Command() = default;
};

// 2) Receiver interface: actions devices can perform
class Device {
public:
    virtual void turnOn() = 0;
    virtual void turnOff() = 0;
    virtual ~Device() = default;
};

// 3) Concrete Receivers
class TV : public Device {
public:
    void turnOn() override { std::cout << "TV is now on\n"; }
    void turnOff() override { std::cout << "TV is now off\n"; }
    void changeChannel() { std::cout << "Channel changed\n"; }
};

class Stereo : public Device {
public:
    void turnOn() override { std::cout << "Stereo is now on\n"; }
    void turnOff() override { std::cout << "Stereo is now off\n"; }
    void adjustVolume() { std::cout << "Volume adjusted\n"; }
};

// 4) Concrete Commands: each holds a receiver and calls its method(s)
class TurnOnCommand : public Command {
    Device* device;
public:
    explicit TurnOnCommand(Device* device) : device(device) {}
    void execute() override { device->turnOn(); }
};

class TurnOffCommand : public Command {
    Device* device;
public:
    explicit TurnOffCommand(Device* device) : device(device) {}
    void execute() override { device->turnOff(); }
};

class AdjustVolumeCommand : public Command {
    Stereo* stereo;
public:
    explicit AdjustVolumeCommand(Stereo* stereo) : stereo(stereo) {}
    void execute() override { stereo->adjustVolume(); }
};

class ChangeChannelCommand : public Command {
    TV* tv;
public:
    explicit ChangeChannelCommand(TV* tv) : tv(tv) {}
    void execute() override { tv->changeChannel(); }
};

// 5) Invoker: knows only how to execute a Command
class RemoteControl {
    Command* command = nullptr; // can be swapped at runtime; non-owning
public:
    void setCommand(Command* c) { command = c; }
    void pressButton() {
        if (!command) throw std::runtime_error("No command set");
        command->execute();
    }
};

// 6) Client: wires everything together and triggers actions
int main() {
    TV tv;
    Stereo stereo;

    TurnOnCommand turnOnTV(&tv);
    TurnOffCommand turnOffTV(&tv);
    AdjustVolumeCommand adjustStereoVolume(&stereo);
    ChangeChannelCommand changeTVChannel(&tv);

    RemoteControl remote;

    remote.setCommand(&turnOnTV);         remote.pressButton();
    remote.setCommand(&adjustStereoVolume); remote.pressButton();
    remote.setCommand(&changeTVChannel);    remote.pressButton();
    remote.setCommand(&turnOffTV);          remote.pressButton();

    return 0;
}
